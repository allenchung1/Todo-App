using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Api.DataTransferObjects;

public record class CreateTodoDto(
    [Required][StringLength(50)] string Name,
    [Required][StringLength(25)] string Category,
    DateTime DueDate
    // bool Complete // remove and default to false in API?
);
