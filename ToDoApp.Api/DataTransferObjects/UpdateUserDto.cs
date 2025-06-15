using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Api.DataTransferObjects;

public record class UpdateUserDto(
    [Required][StringLength(50)] string Username,
    [Required][EmailAddress] string Email,
    [Required][MinLength(6)] string Password,
    [Required][StringLength(25)] string FirstName,
    [Required][StringLength(25)] string LastName
);
