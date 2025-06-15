using ToDoApp.Api.DataTransferObjects;

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

        group.MapGet("/", () => users);

        group.MapGet("/{id}", (int id) =>
        {
            UserDto? user = users.Find((user) => user.Id == id);
            return user is null ? Results.NotFound() : Results.Ok(user);
        }).WithName(GetUserById);

        group.MapPost("/", (CreateUserDto newUser) =>
        {
        UserDto user = new(
            users.Count + 1,
            newUser.Username,
            newUser.Email, //check for unique email
            newUser.FirstName,
            newUser.LastName,
            DateTime.UtcNow
            );

            users.Add(user);
            return Results.AcceptedAtRoute(GetUserById, new { id = user.Id }, user);
        });

        group.MapPut("/{id}", (int id, UpdateUserDto updatedUser) =>
        {
            int index = users.FindIndex((user) => user.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            users[index] = new UserDto(
                id,
                updatedUser.Username,
                updatedUser.Email,
                updatedUser.FirstName,
                updatedUser.LastName,
                users[index].CreatedAt
            );

            return Results.NoContent();
        });

        group.MapDelete("/{id}", (int id) =>
        {
            users.RemoveAll((user) => user.Id == id);

            return Results.NoContent();
        });

        return group;
    }
}
