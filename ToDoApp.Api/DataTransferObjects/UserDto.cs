namespace ToDoApp.Api.DataTransferObjects;

public record class UserDto(
    int Id,
    string Username,
    string Email,
    string FirstName,
    string LastName,
    DateTime CreatedAt
);
