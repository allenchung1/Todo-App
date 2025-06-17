using ToDoApp.Api.Data;
using ToDoApp.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("TaskStore"); //put sosmewhere elsse?
builder.Services.AddSqlite<TodoStoreContext>(connString); //dependency injection?
// builder.Services.AddDbContext<TaskStoreContext>(options => options.UseSqlite(connString));


var app = builder.Build();



app.MapGet("/", () => "Hello World!");

app.MapTodosEndpoints();
app.MapUserEndpoints();

app.MigrateDb();

app.Run();
