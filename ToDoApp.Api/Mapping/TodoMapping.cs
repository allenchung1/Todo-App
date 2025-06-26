using ToDoApp.Api.DataTransferObjects;
using ToDoApp.Api.Entities;

namespace ToDoApp.Api.Mapping;

public static class TodoMapping
{
    public static Todo ToEntity(this CreateTodoDto todo)
    {
        return new Todo() //mapping incomplete, user and userId fields
        {
            Name = todo.Name,
            Category = todo.Category,
            DueDate = todo.DueDate,
            Complete = false
        };
    }

    public static TodoDto ToDto(this TodoDto todo)
    {
        return new TodoDto(
            todo.Id,
            todo.Name,
            todo.Category,
            todo.DueDate,
            todo.Complete
        );
    }
}
