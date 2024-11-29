using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using To_Do_List.Utils;

namespace To_Do_List.DTOs;

public class AddTaskDTO
{
    [StringLength(255)]
    public required string Title { get; set; }
    public string? Description { get; set; }

    public DateOnly? DueDate { get; set; }

    public bool IsCompleted { get; set; } = false;
    [Required]
    public Priority Priority { get; set; }
}
