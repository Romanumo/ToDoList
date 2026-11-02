using TodoList.Data;

var builder = WebApplication.CreateBuilder(args);
// Adding the singleton. Its if convenient as a centralized place to store service
builder.Services.AddSingleton<TodoService>();
var app = builder.Build();
TodoSeeder.Seed(app.Services.GetRequiredService<TodoService>());

app.MapGet("/", (TodoService service) =>
{
    String text = "";
    TodoData[] list = service.GetAll();
    foreach(TodoData item in list)
    {
        text += item.GetTask() + "\n";
    }
    return text;
});

app.MapGet("/{id}", (int id, TodoService service) =>
{
    TodoData? task = service.Get(id);
    if(task == null) return Results.NotFound();

    return Results.Ok(task.GetTask());
});

app.MapPost("/", (CreateTodoRequest addedTask, TodoService service) =>
{
    if(addedTask == null || addedTask.task == "") return Results.BadRequest("Null Task");

    service.Add(addedTask.task);
    return Results.Ok($"Created task: {addedTask.task}");
});

app.MapPut("/{id}", (UpdateTodoRequest changedTask, int id, TodoService service) =>
{
    TodoData? task = service.Get(id);
    if(task == null) return Results.NotFound("Task with id:" + id + " doesnt exist");

    task.SetTask(changedTask.task);
    return Results.Ok();
});

app.MapDelete("/{id}", (int id, TodoService service) =>
{
    if(!service.Delete(id)) return Results.Conflict("Task with id:" + id + " doesnt exist");

    return Results.NoContent();
});

app.Run();
