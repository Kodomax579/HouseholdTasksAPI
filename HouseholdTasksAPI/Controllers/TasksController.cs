using HouseholdTasksAPI.Database;
using HouseholdTasksAPI.DataModels;
using HouseholdTasksAPI.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HouseholdTasksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly IHubContext<TaskHub> _hubContext;
        private Context _context;

        public TasksController(IHubContext<TaskHub> hubContext, Context context)
        {
            _hubContext = hubContext;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] Tasks newTask)
        {
            try
            {
                if (newTask == null)
                {
                    return BadRequest("Body is empty");
                }

                await _context.Tasks.AddAsync(newTask);
                await _context.SaveChangesAsync();

                await _hubContext.Clients.All.SendAsync("TaskAdded", newTask);

                return Ok(newTask);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> CompleteTask(int id)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                {
                    return NotFound("Task not found");
                }

                task.IsCompleted = !task.IsCompleted;
                await _context.SaveChangesAsync();

                await _hubContext.Clients.All.SendAsync("TaskCompleted", task);

                return NoContent();

            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [HttpGet("/allTasks")]
        public async Task<ActionResult<IEnumerable<Tasks>>> GetAllTasks()
        {
            try
            {
                return Ok(await _context.Tasks.ToListAsync());
            }
            catch(Exception ex) 
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound(); 
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("TaskDeleted", id);

            return NoContent();
        }
    }
}
