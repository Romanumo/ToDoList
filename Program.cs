using TodoList.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Adding the singleton. It is convenient as a centralized place to store service
builder.Services.AddSingleton<TodoService>();
builder.Services.AddOpenApi();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

TodoSeeder.Seed(app.Services.GetRequiredService<TodoService>());

var todos = app.MapGroup("/todos");

todos.MapGet("/", (TodoService service) =>
{
    // Returns JSON response
    return service.GetAll().Select(t => new TodoResponse(t.id, t.task));
});

todos.MapGet("/{id:int}", (int id, TodoService service) =>
{
    TodoData? task = service.Get(id);
    if(task == null) return Results.NotFound();

    return TypedResults.Ok(new TodoResponse(task.id, task.task));
});

todos.MapPost("/", (CreateTodoRequest addedTask, TodoService service) =>
{
    if(addedTask == null || String.IsNullOrWhiteSpace(addedTask.task)) return Results.BadRequest("Empty Task");

    var item = service.Add(addedTask.task);
    return TypedResults.Created($"/todos/{item.id}", new TodoResponse(item.id, item.task));
});

todos.MapPut("/{id:int}", (UpdateTodoRequest changedTask, int id, TodoService service) =>
{
    TodoData? item = service.Update(id, changedTask.task);
    if(item == null) return Results.NotFound(id);
    return TypedResults.NoContent();
});

todos.MapDelete("/{id:int}", (int id, TodoService service) =>
{
    if(!service.Delete(id)) return Results.NotFound(id);

    return TypedResults.NoContent();
});

app.Run();
