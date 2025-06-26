namespace ToDoApp.Api.DataTransferObjects;

public record class TodoDto(
    int Id,
    int UserId,
    string Name,
    string Category,
    DateTime DueDate,
    bool Complete
);
