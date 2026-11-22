using System.Collections.Concurrent;

namespace TodoList.Data;

public class TodoService
{
    private readonly ConcurrentDictionary<int, TodoData> items = new();
    
    private int nextID;

    public TodoData? Get(int id) => items.TryGetValue(id, out var item) ? item : null;

    public int GetCount() => items.Count;

    public TodoData[] GetAll() => items.Values.ToArray();

    public TodoData Add(String task)
    {
        var newID = Interlocked.Increment(ref nextID);
        TodoData item = new TodoData(task, newID);
        items[newID] = item;
        return item;
    }

    public TodoData? Update(int id, String task, bool isFinished = false)
    {
        // Get returns the reference to the data within the list
        var todoItem = Get(id);
        if(todoItem == null) return null;

        todoItem.SetTask(task);
        todoItem.SetIsFinished(isFinished);
        return todoItem;
    }

    public bool Delete(int id) => items.TryRemove(id, out _);
}

public static class TodoSeeder
{
    public static void Seed(TodoService service)
    {
        service.Add("Clean up the room");
        service.Add("Cook food");
    }
}