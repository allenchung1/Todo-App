using Microsoft.EntityFrameworkCore;
using ToDoApp.Api.Data;
using ToDoApp.Api.DataTransferObjects;
using ToDoApp.Api.Entities;
using ToDoApp.Api.Mapping;

namespace ToDoApp.Api.Endpoints;

public static class TodosEndpoints
{
    const string GetTodoById = "GetTodoById";

    public static RouteGroupBuilder MapTodosEndpoints(this WebApplication app) // extension method of WebApplication class
    {
        var group = app.MapGroup("todos").WithParameterValidation();
        
        // GET /todos
        group.MapGet("/", async (TodoStoreContext dbContext) => await dbContext.Todos.Select(todo => todo.ToDto()).AsNoTracking().ToListAsync()); // auth

        // GET /todos/1
        group.MapGet("/{id}", async (int id, TodoStoreContext dbContext) => // auth
        {
            Todo? todo = await dbContext.Todos.FindAsync(id);
            return todo is null ? Results.NotFound() : Results.Ok(todo);
        }).WithName(GetTodoById);

        group.MapGet("/user/{userId}", async (int userId, TodoStoreContext dbContext) => // auth
        {
            var todos = await dbContext.Todos.Where(todo => todo.UserId == userId).Select(todo => todo.ToDto()).ToListAsync();
            return Results.Ok(todos);
        });

        group.MapPost("/", async (CreateTodoDto newTodo, TodoStoreContext dbContext) => // auth
        {
            Todo todo = newTodo.ToEntity();

            dbContext.Todos.Add(todo);
            await dbContext.SaveChangesAsync();

            return Results.AcceptedAtRoute(
                GetTodoById,
                new { id = todo.Id },
                todo.ToDto()
            );
        });

        group.MapPut("/{id}", async (int id, UpdateTodoDto updatedTodo, TodoStoreContext dbContext) => // auth
        {
            var existingTodo = await dbContext.Users.FindAsync(id);

            if (existingTodo is null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingTodo).CurrentValues.SetValues(updatedTodo.ToEntity());
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (int id, TodoStoreContext dbContext) => // auth
        {
            await dbContext.Todos.Where(todo => todo.Id == id).ExecuteDeleteAsync();

            return Results.NoContent();
        });

        return group;
    }
}
