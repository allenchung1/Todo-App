using ToDoApp.Api.DataTransferObjects;

namespace ToDoApp.Api.Endpoints;

public static class TodosEndpoints
{
    const string GetTodoById = "GetTodoById";

    private static readonly List<TodoDto> todos = [
        new (
            1,
            "take out the trash",
            "chores",
            DateTime.UtcNow.AddDays(1),
            false
        ),
        new (
            2,
            "do the dishes",
            "chores",
            DateTime.UtcNow.AddDays(2),
            false
        ),
        new (
            3,
            "do laundry",
            "chores",
            DateTime.UtcNow.AddDays(3),
            false
        ),
        new (
            4,
            "mow the lawn",
            "chores",
            DateTime.UtcNow.AddDays(4),
            false
        ),
        new (
            5,
            "vacuum",
            "chores",
            DateTime.UtcNow.AddDays(5),
            false
        ),

    ];

    public static RouteGroupBuilder MapTodosEndpoints(this WebApplication app) // extension method of WebApplication class
    {
        var group = app.MapGroup("todos").WithParameterValidation();
        
        // GET /todos
        group.MapGet("/", () => todos); // /games/

        // GET /todos/1
        group.MapGet("/{id}", (int id) =>
        {
            TodoDto? todo = todos.Find(todo => todo.Id == id);
            return todo is null ? Results.NotFound() : Results.Ok(todo);
        }).WithName(GetTodoById);

        group.MapPost("/", (CreateTodoDto newTodo) =>
        {
            TodoDto todo = new(
                todos.Count + 1,
                newTodo.Name,
                newTodo.Category,
                newTodo.DueDate,
                newTodo.Complete
            );
            todos.Add(todo);
            return Results.AcceptedAtRoute(GetTodoById, new { id = todo.Id }, todo);
        });

        group.MapPut("/{id}", (int id, UpdateTodoDto updatedTodo) =>
        {
            int index = todos.FindIndex((todo) => todo.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }
            todos[index] = new TodoDto(
                id,
                updatedTodo.Name,
                updatedTodo.Category,
                updatedTodo.DueDate,
                updatedTodo.Complete
            );

            return Results.NoContent();
        });

        group.MapDelete("/{id}", (int id) =>
        {
            todos.RemoveAll(todo => todo.Id == id);

            return Results.NoContent();
        });

        return group;
    }
}
