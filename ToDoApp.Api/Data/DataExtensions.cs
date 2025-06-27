using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Api.Data;

public static class DataExtensions
{
    public static async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TodoStoreContext>();
        await dbContext.Database.MigrateAsync();
    }
}
