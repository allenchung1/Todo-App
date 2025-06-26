using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using ToDoApp.Api.Data;
using ToDoApp.Api.DataTransferObjects;
using ToDoApp.Api.Endpoints;
using ToDoApp.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!))
    };
});
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];
Console.WriteLine($"Secret key: {secretKey}");
builder.Services.AddAuthorization();

var connString = builder.Configuration.GetConnectionString("TaskStore"); //put sosmewhere elsse?
builder.Services.AddSqlite<TodoStoreContext>(connString); //dependency injection?

builder.Services.AddScoped<JwtTokenService>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.MapTodosEndpoints();
app.MapUserEndpoints();

app.MigrateDb();

app.Run();
