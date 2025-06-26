using ToDoApp.Api.DataTransferObjects;
using ToDoApp.Api.Entities;

namespace ToDoApp.Api.Mapping;

public static class TodoMapping
{
    public static Todo ToEntity(this CreateTodoDto todo)
    {
        return new Todo()
        {
            UserId = todo.UserId,
            Name = todo.Name,
            Category = todo.Category,
            DueDate = todo.DueDate,
            Complete = false
        };
    }

    public static Todo ToEntity(this UpdateTodoDto todo)
    {
        return new Todo()
        {
            Name = todo.Name,
            Category = todo.Category,
            DueDate = todo.DueDate,
            Complete = todo.Complete
        };
    }

    public static TodoDto ToDto(this Todo todo)
    {
        return new TodoDto(
            todo.Id,
            todo.UserId,
            todo.Name,
            todo.Category,
            todo.DueDate,
            todo.Complete
        );
    }
}
