using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using To_Do_List.Utils;

namespace To_Do_List.Models;

public class MyTask
{
    public int Id { get; set; }

    [StringLength(255)]
    public required string Title { get; set; }

    public string? Description { get; set; }

    [Column(TypeName = "date")]
    public DateOnly? DueDate { get; set; }

    public bool IsCompleted { get; set; } = false;

    [Required]
    public Priority Priority { get; set; }

    [Column(TypeName = "date")]
    public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Today);
}
