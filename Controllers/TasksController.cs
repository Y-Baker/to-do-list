using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using To_Do_List.DTOs;
using To_Do_List.Models;
using To_Do_List.Utils;

namespace To_Do_List.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly UnitOfWork unit;
    private readonly IMapper mapper;

    public TasksController(UnitOfWork unit, IMapper _mapper)
    {
        this.unit = unit;
        mapper = _mapper;
    }

    [SwaggerOperation("Get All Tasks", "Return a list of all tasks in the system.")]
    [SwaggerResponse(200, "Successfully", typeof(List<MyTask>))]
    [Produces("application/json")]
    [HttpGet]
    public IActionResult GetAll()
    {
        List<MyTask> tasks = unit.Tasks.SelectAll();

        return Ok(tasks);
    }

    [SwaggerOperation("Get Task By Id", "Return an specific task based on its id given in route in the system.")]
    [SwaggerResponse(200, "Successfully", typeof(MyTask))]
    [SwaggerResponse(404, "Failed, Id Not Found")]
    [Produces("application/json")]
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        MyTask? task = unit.Tasks.SelectById(id);
        if (task == null)
            return BadRequest();
        return Ok(task);
    }


    [SwaggerOperation("Add New Task", "Add New Task given in the body to the system.")]
    [SwaggerResponse(201, "Successfully Added", typeof(AddTaskDTO))]
    [SwaggerResponse(400, "Failed")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [HttpPost]
    public IActionResult Add(AddTaskDTO taskDTO)
    {
        if (taskDTO is null)
            return BadRequest();

        if (!ModelState.IsValid)
            return BadRequest();

        MyTask task = mapper.Map<MyTask>(taskDTO);

        unit.Tasks.Add(task);
        unit.Save();
        return CreatedAtAction(nameof(Get), new { id = task.Id }, task);
    }

    [SwaggerOperation("Update Task", "Update an Exists task given in the body to the system.")]
    [SwaggerResponse(204, "Successfully Update")]
    [SwaggerResponse(404, "Failed Not Exists, Id not found")]
    [SwaggerResponse(400, "Failed Wronge Entry")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [HttpPut("{id}")]
    public IActionResult Put(int id, UpdateTaskDTO taskDTO)
    {
        if (taskDTO is null)
            return BadRequest();
        if (taskDTO.Id != id)
            return BadRequest();
        if (!ModelState.IsValid)
            return BadRequest();

        MyTask? taskBefore = unit.Tasks.SelectById(id, track: false);
        if (taskBefore is null)
            return NotFound();

        MyTask task = mapper.Map<MyTask>(taskDTO, context =>
        {
            context.Items["CreatedAt"] = taskBefore.CreatedAt;
        });
        
        unit.Tasks.Update(task);
        unit.Save();
        
        return NoContent();
    }


    [SwaggerOperation("Delete Product", "Delete an Existing Task by an id given in the rout from the system")]
    [SwaggerResponse(200, "Successfully Deleted", typeof(List<MyTask>))]
    [SwaggerResponse(404, "Product Not Exists")]
    [Produces("application/json")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        MyTask? task = unit.Tasks.SelectById(id);
        if (task is null)
            return BadRequest();

        unit.Tasks.Delete(task);
        unit.Save();

        return GetAll();
    }

    [SwaggerOperation("Set Task Completion", "Set an Exists task given in the body to the system to a Complete State")]
    [SwaggerResponse(204, "Successfully Set")]
    [SwaggerResponse(404, "Failed Not Exists, Id not found")]
    [Produces("application/json")]
    [HttpPut("{id}/complete")]
    public IActionResult Complete(int id)
    {
        try
        {
            unit.Tasks.AssignComplete(id, true);
            unit.Save();
            return NoContent();
        }
        catch
        {
            return NotFound();
        }
    }

    [SwaggerOperation("Clear Task Completion", "CLear an Exists task given in the body to the system to Incomplete State")]
    [SwaggerResponse(204, "Successfully Set")]
    [SwaggerResponse(404, "Failed Not Exists, Id not found")]
    [Produces("application/json")]
    [HttpPut("{id}/incomplete")]
    public IActionResult InComplete(int id)
    {
        try
        {
            unit.Tasks.AssignComplete(id, false);
            unit.Save();
            return NoContent();
        }
        catch
        {
            return NotFound();
        }
    }

    [SwaggerOperation("Update Task Priority", "Update an Exists task given in the body to the system to a given Priority State")]
    [SwaggerResponse(204, "Successfully Update")]
    [SwaggerResponse(404, "Failed Not Exists, Id not found")]
    [Produces("application/json")]
    [HttpPut("{id}/priority")]
    public IActionResult UpdatePriority(int id, Priority priority)
    {
        MyTask? task = unit.Tasks.SelectById(id);
        if (task is null)
            return BadRequest();

        task.Priority = priority;

        unit.Save();
        return NoContent();
    }
}
