using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Api.DataTransferObjects;

public record class LoginDto(
    [Required] string Email,
    [Required] string Password
);