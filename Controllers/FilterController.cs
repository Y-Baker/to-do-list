using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using To_Do_List.Models;

namespace To_Do_List.Controllers;

[Route("api/Tasks/filter")]
[ApiController]
public class FilterController : ControllerBase
{
    private readonly UnitOfWork unit;

    public FilterController(UnitOfWork unit)
    {
        this.unit = unit;
    }

    [SwaggerOperation("Get Tasks Filtered", "Return a list of tasks after filter task in the system.")]
    [SwaggerResponse(200, "Successfully", typeof(List<MyTask>))]
    [Produces("application/json")]
    [HttpGet]
    public IActionResult Filter([FromQuery] string? completed = null, [FromQuery] string? due_date = null, [FromQuery] string? priority = null)
    {
        DateOnly? due = null;
        DateOnly dueDate;

        if (due_date is not null)
        {
            if (!DateOnly.TryParse(due_date, out dueDate))
                return BadRequest();
        }

        bool? isCompleted = !string.IsNullOrEmpty(completed) ? bool.Parse(completed) : null;
        To_Do_List.Utils.Priority? taskPriority = !string.IsNullOrEmpty(priority) ? (To_Do_List.Utils.Priority)Enum.Parse(typeof(To_Do_List.Utils.Priority), priority) : null;

        List<MyTask>? tasks = unit.Tasks.SelectAll(isCompleted, due, taskPriority);

        return Ok(tasks);
    }
}
