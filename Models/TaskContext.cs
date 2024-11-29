using Microsoft.EntityFrameworkCore;

namespace To_Do_List.Models;

public class TaskContext : DbContext
{
    public virtual DbSet<MyTask> Tasks { get; set; }

    public TaskContext(DbContextOptions<TaskContext> options) : base(options)
    {
    }
}
