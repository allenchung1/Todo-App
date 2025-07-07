using Microsoft.EntityFrameworkCore;
using ToDoApp.Api.Data;
using ToDoApp.Api.DataTransferObjects;
using ToDoApp.Api.Entities;
using ToDoApp.Api.Mapping;
using ToDoApp.Api.Services;

namespace ToDoApp.Api.Endpoints;

public static class UserEndpoints
{
    const string GetUserById = "GetUserById";

    public static RouteGroupBuilder MapUserEndpoints(this WebApplication app)
    {

        var group = app.MapGroup("users").WithParameterValidation();

        group.MapGet("/", async (TodoStoreContext dbContext) => await dbContext.Users/*.Include()*/.Select(user => user.ToDto()).AsNoTracking().ToListAsync());

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
            await dbContext.SaveChangesAsync();

            return Results.AcceptedAtRoute(
                GetUserById,
                new { id = user.Id },
                user.ToDto()
            );
        });

        group.MapPut("/{id}", async (int id, UpdateUserDto updatedUser, TodoStoreContext dbContext) => //auth
        {
            var existingUser = await dbContext.Users.FindAsync(id);

            if (existingUser is null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingUser).CurrentValues.SetValues(updatedUser.ToEntity(id));
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (int id, TodoStoreContext dbContext) => //auth
        {
            await dbContext.Users.Where(user => user.Id == id).ExecuteDeleteAsync(); //batch delete

            return Results.NoContent();
        });

        // TODO:
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
