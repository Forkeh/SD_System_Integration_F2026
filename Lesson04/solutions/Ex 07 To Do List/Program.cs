using Ex_07_To_Do_List.Data;
using Ex_07_To_Do_List.Helpers;
using Ex_07_To_Do_List.Models;
using Microsoft.Data.Sqlite;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

Console.WriteLine(BCrypt.Net.BCrypt.HashPassword("secret"));

// --- Infrastructure setup ---
var sqliteConn = new SqliteConnection("Data Source=Data/users.db");
sqliteConn.Open();
await new SqliteCommand("""
                        CREATE TABLE IF NOT EXISTS users (
                        username TEXT PRIMARY KEY,
                        password_hash TEXT NOT NULL
                        )
                        """, sqliteConn).ExecuteNonQueryAsync();

// Seed a test user (password: "secret")
new SqliteCommand("""
                      INSERT OR IGNORE INTO users VALUES ('brian', '$2a$11$4Rz6uv02Vk3IlfdcSrkSX.7mLn1VFsZ9wFohcuvkEffn6POMIZfzm')
                  """, sqliteConn).ExecuteNonQuery();

var redis = ConnectionMultiplexer.Connect("localhost:6379").GetDatabase();

var helpers = new Helpers(sqliteConn, redis);

// --- Auth middleware ---
app.Use(async (ctx, next) =>
{
    var path = ctx.Request.Path.Value ?? "";
    if (path.StartsWith("/auth"))
    {
        await next.Invoke(ctx); // skip auth for /auth/login and /auth/logout
        return;
    }

    var header = ctx.Request.Headers.Authorization.ToString();

    if (!header.StartsWith("Bearer "))
    {
        ctx.Response.StatusCode = 401;
        return;
    }

    var token = header["Bearer ".Length..];

    var username = helpers.GetUsernameFromToken(token);

    if (username is null)
    {
        ctx.Response.StatusCode = 401;
        return;
    }

    await next.Invoke(ctx);
});

// --- Auth endpoints ---
app.MapPost("/auth/login", (LoginRequest req) =>
{
    if (!helpers.VerifyUser(req.Username, req.Password))
    {
        return Results.Unauthorized();
    }

    var token = helpers.GenerateToken();
    redis.StringSet($"token:{token}", req.Username, TimeSpan.FromHours(8));

    return Results.Ok(new { token });
});

app.MapPost("/auth/logout", (HttpContext ctx) =>
{
    var token = ctx.Request.Headers.Authorization.ToString()["Bearer ".Length..];
    redis.KeyDelete($"token:{token}");
    return Results.NoContent();
});


app.MapGet("/tasks", (int page = 1, int size = 5) =>
{
    var paged = Tasks.TasksList
        .Skip((page - 1) * size)
        .Take(size)
        .ToList();

    return Results.Ok(new
    {
        Page = page,
        Size = size,
        TotalCount = Tasks.TasksList.Count,
        TotalPages = (int)Math.Ceiling(Tasks.TasksList.Count / (double)size),
        Data = paged
    });
}).WithName("GetTasks");

app.MapGet("/tasks/{id}", (int id) =>
{
    var task = Tasks.TasksList.FirstOrDefault(t => t.Id == id);
    return task is null ? Results.NotFound() : Results.Ok(task);
}).WithName("GetTaskById");

app.MapPost("/tasks", (TaskDto task) =>
{
    task.Links = TaskDto.GenerateLinks(task.Id);
    Tasks.TasksList.Add(task);
    return Results.Created($"/tasks/{task.Id}", task);
}).WithName("CreateTask");

app.MapPut("/tasks/{id}", (int id, TaskDto updatedTask) =>
{
    var index = Tasks.TasksList.FindIndex(t => t.Id == id);
    if (index == -1)
    {
        return Results.NotFound();
    }

    updatedTask.Links = TaskDto.GenerateLinks(id);
    Tasks.TasksList[index] = updatedTask;
    return Results.Ok(updatedTask);
}).WithName("UpdateTaskById");

app.MapDelete("/tasks/{id}", (int id) =>
{
    var task = Tasks.TasksList.FirstOrDefault(t => t.Id == id);
    if (task is null)
    {
        return Results.NotFound();
    }

    Tasks.TasksList.Remove(task);
    return Results.NoContent();
}).WithName("DeleteTaskById");

app.Run();