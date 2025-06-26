namespace ToDoApp.Api.DataTransferObjects;

public record class TodoDto(
    int Id,
    string Name, //userid
    string Category,
    DateTime DueDate,
    bool Complete
);
