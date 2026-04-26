using HouseholdTasksAPI.Database;
using HouseholdTasksAPI.DataModels;
using HouseholdTasksAPI.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace HouseholdTasksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MealController : ControllerBase
    {
        private readonly IHubContext<TaskHub> _hubContext;
        private Context _context;

        public MealController(IHubContext<TaskHub> hubContext, Context context)
        {
            _hubContext = hubContext;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreatMealSuggestion([FromBody] MealSuggestion newMeal)
        {
            try
            {
                if (newMeal == null)
                {
                    return BadRequest("Body is empty");
                }

                await _context.MealSuggestions.AddAsync(newMeal);
                await _context.SaveChangesAsync();

                await _hubContext.Clients.All.SendAsync("MealSuggestionAdded", newMeal);

                return Ok(newMeal);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [HttpPut("{id}/isActiv")]
        public async Task<IActionResult> ActivateMeal(int id)
        {
            try
            {
                var meal = await _context.MealSuggestions.FindAsync(id);
                if (meal == null)
                {
                    return NotFound("Task not found");
                }

                meal.IsActiv = !meal.IsActiv;
                await _context.SaveChangesAsync();

                await _hubContext.Clients.All.SendAsync("ActivateMealSuggestionComplete", id);

                return NoContent();

            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [HttpGet("/allMealSuggestion")]
        public async Task<ActionResult<List<MealSuggestion>>> GetAllMealSuggestion()
        {
            try
            {
                return Ok(await _context.MealSuggestions.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteMeal(int id)
        {
            var meal = await _context.MealSuggestions.FindAsync(id);

            if (meal == null)
            {
                return NotFound();
            }

            _context.MealSuggestions.Remove(meal);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("TaskDeleted", id);

            return NoContent();
        }
    }
}
