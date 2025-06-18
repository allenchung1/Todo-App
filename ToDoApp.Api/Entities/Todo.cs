using System;

namespace ToDoApp.Api.Entities;

public class Todo
{
    public int Id { get; set; }
    public required string Name { get; set; }

    // Create an association using Entity Framework Core to make a 1 to many relationship
    public int UserId { get; set; }
    public User? User { get; set; }

    public required string Category { get; set; }
    public DateTime DueDate { get; set; }

    public bool Complete { get; set; }
}
