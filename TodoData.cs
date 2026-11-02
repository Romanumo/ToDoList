namespace TodoList.Data;

public class TodoData
{
    public String task {get; private set;}
    public int id {get;}


    public TodoData(String task, int id)
    {
        this.task = task;
        this.id = id;
    }

    public String GetTask() => "Task " + id + ": " + task;

    public void SetTask(String task) => this.task = task;
}

// Simple DTO (Data Transfer Objects) - Tackles the boundary transfer of information
public record UpdateTodoRequest (String task);
public record CreateTodoRequest (String task);