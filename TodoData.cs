namespace TodoList.Data;

public class TodoData
{
    public String task {get; private set;}
    public int id {get;}

    static int idCounter = 0;

    public TodoData(String task)
    {
        this.task = task;
        id = idCounter;
        idCounter++;
    }

    public String GetTask() => "Task " + id + ": " + task;

    public void SetTask(String task) => this.task = task;
}

public record UodateTodoRequest (int id, String task);
public record CreateTodoRequest (String task);