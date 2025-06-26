using System;

namespace ToDoApp.Api.Entities;

public class User
{
    public int Id { get; set; }

    public required string Username { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public DateTime CreatedAt { get; set; }
    
    //public List<Todo> Todos { get; set; } = new();
}
