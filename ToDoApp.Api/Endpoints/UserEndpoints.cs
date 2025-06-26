using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ToDoApp.Api.Data;
using ToDoApp.Api.DataTransferObjects;
using ToDoApp.Api.Entities;
using ToDoApp.Api.Mapping;
using ToDoApp.Api.Services;

namespace ToDoApp.Api.Endpoints;

public static class UserEndpoints
{
    const string GetUserById = "GetUserById";

    private static readonly List<UserDto> users = [
        new (
            1,
            "allenchung1",
            "allenchung75@gmail.com",
            "Allen",
            "Chung",
            DateTime.UtcNow.AddDays(0)
        ),
        new (
            2,
            "BETH19",
            "elizabethlee@gmail.com",
            "Elizabeth",
            "Lee",
            DateTime.UtcNow.AddDays(1)
        ),
    ];
    public static RouteGroupBuilder MapUserEndpoints(this WebApplication app)
    {

        var group = app.MapGroup("users").WithParameterValidation();

        group.MapGet("/", (TodoStoreContext dbContext) => dbContext.Users/*.Include()*/.Select(user => user.ToDto()).AsNoTracking());

        group.MapGet("/{id}", async (int id, TodoStoreContext dbContext) =>
        {
            User? user = await dbContext.Users.FindAsync(id);
            return user is null ? Results.NotFound() : Results.Ok(user.ToDto());
        }).WithName(GetUserById);

        group.MapPost("/", async (CreateUserDto newUser, TodoStoreContext dbContext) => //Minimal APIs use automatic model binding with JSON deserialization
        {
            var found = await dbContext.Users.AnyAsync(user => user.Email == newUser.Email);
            if (found)
            {
                return Results.BadRequest("Email is already in use.");
            }

            User user = newUser.ToEntity();

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            return Results.AcceptedAtRoute(
                GetUserById,
                new { id = user.Id },
                user.ToDto()
            );
        });

        group.MapPut("/{id}", (int id, UpdateUserDto updatedUser, TodoStoreContext dbContext) => //auth
        {
            var existingUser = dbContext.Users.Find(id);

            if (existingUser is null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingUser).CurrentValues.SetValues(updatedUser.ToEntity(id));
            dbContext.SaveChanges();

            return Results.NoContent();
        });

        group.MapDelete("/{id}", (int id, TodoStoreContext dbContext) => //auth
        {
            dbContext.Users.Where(user => user.Id == id).ExecuteDelete(); //batch delete
            dbContext.SaveChanges();

            return Results.NoContent();
        });

        // group.MapPost("/login", (LoginDto login) =>
        // {
        //     var user = users.Find(u => u.Username == LoginDto.Username && u.Password == loginDto.Password);
        //     if (user is null)
        //     {
        //         return Results.Unauthorized();
        //     }

        //     string token = tokenService.GenerateToken(user.Username, user.Id);
        //     return Results.Ok(new { token });
        // });

        app.MapPost("/login", (LoginDto loginDto, JwtTokenService tokenService) =>
        {

            return new
            {
                access_token = tokenService.GenerateToken(loginDto.Email, loginDto.Password)
            };
        });

        
        return group;
    }
}
