using Microsoft.EntityFrameworkCore;
using ToDoApp.Api.Data;
using ToDoApp.Api.DataTransferObjects;
using ToDoApp.Api.Entities;
using ToDoApp.Api.Mapping;

namespace ToDoApp.Api.Endpoints;

public static class TodosEndpoints
{
    const string GetTodoById = "GetTodoById";

    // private static readonly List<TodoDto> todos = [
    //     new (
    //         1,
    //         "take out the trash",
    //         "chores",
    //         DateTime.UtcNow.AddDays(1),
    //         false
    //     ),
    //     new (
    //         2,
    //         "do the dishes",
    //         "chores",
    //         DateTime.UtcNow.AddDays(2),
    //         false
    //     ),
    //     new (
    //         3,
    //         "do laundry",
    //         "chores",
    //         DateTime.UtcNow.AddDays(3),
    //         false
    //     ),
    //     new (
    //         4,
    //         "mow the lawn",
    //         "chores",
    //         DateTime.UtcNow.AddDays(4),
    //         false
    //     ),
    //     new (
    //         5,
    //         "vacuum",
    //         "chores",
    //         DateTime.UtcNow.AddDays(5),
    //         false
    //     ),

    // ];

    public static RouteGroupBuilder MapTodosEndpoints(this WebApplication app) // extension method of WebApplication class
    {
        var group = app.MapGroup("todos").WithParameterValidation();
        
        // GET /todos
        group.MapGet("/", (TodoStoreContext dbContext) => dbContext.Todos.Select(todo => todo.ToDto()).AsNoTracking()); // auth

        // GET /todos/1
        group.MapGet("/{id}", async (int id, TodoStoreContext dbContext) => // add jwt authorization
        {
            Todo? todo = await dbContext.Todos.FindAsync(id);
            return todo is null ? Results.NotFound() : Results.Ok(todo);
        }).WithName(GetTodoById);

        group.MapGet("/user/{userId}", async (int userId, TodoStoreContext dbContext) => //auth
        {
            var todos = await dbContext.Todos.Where(todo => todo.UserId == userId).Select(todo => todo.ToDto()).ToListAsync();
            return Results.Ok(todos);
        });

        group.MapPost("/", (CreateTodoDto newTodo, TodoStoreContext dbContext) => // auth
        {
            Todo todo = newTodo.ToEntity();

            dbContext.Todos.Add(todo);
            dbContext.SaveChanges();

            return Results.AcceptedAtRoute(
                GetTodoById,
                new { id = todo.Id },
                todo.ToDto()
            );
        });

        group.MapPut("/{id}", async (int id, UpdateTodoDto updatedTodo, TodoStoreContext dbContext) => //add jwt authorization
        {
            var existingTodo = await dbContext.Users.FindAsync(id);

            if (existingTodo is null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingTodo).CurrentValues.SetValues(updatedTodo.ToEntity());
            dbContext.SaveChanges();

            return Results.NoContent();
        });

        group.MapDelete("/{id}", (int id, TodoStoreContext dbContext) => //add jwt authorization
        {
            dbContext.Todos.Where(todo => todo.Id == id).ExecuteDelete();
            dbContext.SaveChanges();

            return Results.NoContent();
        });

        return group;
    }
}
