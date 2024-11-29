using To_Do_List.Models;

namespace To_Do_List;

public class UnitOfWork
{
    private readonly TaskContext db;
    private Repository? tasks;

    public UnitOfWork(TaskContext db)
    {
        this.db = db;
    }

    public Repository Tasks
    {
        get
        {
            if (tasks == null)
            {
                tasks = new Repository(db);
            }
            return tasks;
        }
    }


    public void Save()
    {
        db.SaveChanges();
    }
}
