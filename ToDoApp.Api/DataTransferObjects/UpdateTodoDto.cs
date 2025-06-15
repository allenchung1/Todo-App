using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Api.DataTransferObjects;

public record class UpdateTodoDto(
    [Required][StringLength(50)] string Name,
    [Required][StringLength(25)] string Category,
    [Required] DateTime DueDate,
    [Required] bool Complete
);