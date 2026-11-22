namespace TodoList.Data;

public class TodoData
{
    public String task {get; private set;}
    public bool isFinished {get; private set;} = false;
    public int id {get;}

    public TodoData(String task, int id, bool isFinished = false)
    {
        this.task = task;
        this.id = id;
        this.isFinished = isFinished;
    }

    public void SetTask(String task) => this.task = task;

    public void SetIsFinished(bool isFinished) => this.isFinished = isFinished;
}

// Simple DTO (Data Transfer Objects) - Tackles the boundary transfer of information
public record CreateTodoRequest (String task);
public record UpdateTodoRequest (String? task, bool? isFinished);
public record TodoResponse (int id, String task, bool isFinished);