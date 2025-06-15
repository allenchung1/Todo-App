using Microsoft.EntityFrameworkCore;
using ToDoApp.Api.Entities;

namespace ToDoApp.Api.Data;

public class TodoStoreContext(DbContextOptions<TodoStoreContext> options) : DbContext(options)
{
    public DbSet<Todo> Todos => Set<Todo>();

    public DbSet<User> Users => Set<User>();
}