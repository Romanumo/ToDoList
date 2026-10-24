using TodoList.Data;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<TodoData> todoList = new List<TodoData>();

todoList.Add(new TodoData("Clean up the room"));
todoList.Add(new TodoData("Cook food"));

app.MapGet("/", () =>
{
    String result = "";
    for(int i = 0;i<todoList.Count;i++)
    {
        result += todoList[i].GetTask() + "\n";
    }
    return result;
});

app.MapGet("/{id}", (int id) =>
{
    TodoData? task = todoList.FirstOrDefault(t => t.id == id);
    if(task == null) return Results.NotFound();

    return Results.Ok(task.GetTask());
});

app.MapPost("/", (TodoData addedTask) =>
{
    if(addedTask == null || addedTask.task == "") return Results.BadRequest("Null Task");

    todoList.Add(addedTask);
    return Results.Ok($"Created task: {addedTask.task}");
});

app.MapPut("/{id}", (int id, TodoData changedTask) =>
{
    TodoData? task = todoList.FirstOrDefault(t => t.id == id);
    if(task == null) return Results.NotFound("Task with id:" + id + " doesnt exist");

    task.SetTask(changedTask.task);
    return Results.Ok($"Changed task: {changedTask.task}");
});

app.MapDelete("/{id}", (int id) =>
{
    TodoData? task = todoList.FirstOrDefault(t => t.id == id);
    if(task == null) return Results.Conflict("Task with id:" + id + " doesnt exist");

    todoList.Remove(task);
    return Results.NoContent();
});

app.Run();
