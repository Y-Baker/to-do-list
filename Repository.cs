using Microsoft.EntityFrameworkCore;
using To_Do_List.Models;
using To_Do_List.Utils;

namespace To_Do_List;

public class Repository
{
    private readonly TaskContext db;

    public Repository(TaskContext db)
    {
        this.db = db;
    }

    public List<MyTask> SelectAll(bool? complete = null, DateOnly? date = null, Priority? priority = null)
    {
        List<MyTask> tasks = db.Tasks.ToList();
        if (complete is not null)
            tasks = tasks.Where(t => t.IsCompleted == complete).ToList();
        if (date is not null)
            tasks = tasks.Where(t => t.DueDate is not null && t.DueDate.Value == date.Value).ToList();
        if (priority is not null)
            tasks = tasks.Where(t => t.Priority == priority.Value).ToList();
        return tasks;
    }

    public MyTask? SelectById(int id, bool track=true)
    {
        if (!track)
            return db.Tasks.AsNoTracking().FirstOrDefault(t => t.Id == id);
        return db.Tasks.Find(id);
    }
    public void Add(MyTask task)
    {
        db.Tasks.Add(task);
    }

    public void Update(MyTask task)
    {
        db.Entry(task).State = EntityState.Modified;
    }

    public void Delete(MyTask task)
    {
        if (task == null)
            throw new ArgumentNullException("Can't Delete Null");

        db.Tasks.Remove(task);
    }

    public void Delete(int id)
    {
        MyTask? entity = db.Tasks.Find(id);

        if (entity is not null)
            Delete(entity);
    }

    public void AssignComplete(int id, bool complete)
    {
        MyTask? task = SelectById(id);

        if (task == null)
            throw new ArgumentException("Not Found");

        task.IsCompleted = complete;
    }
}
