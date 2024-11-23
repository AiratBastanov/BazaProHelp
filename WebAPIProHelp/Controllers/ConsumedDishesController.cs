using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAPIProHelp.Models;

namespace WebAPIProHelp.Controllers
{
    [Route("glutenappapi/consumeddish")]
    [ApiController]
    public class ConsumedDishesController : Controller
    {
        private readonly GlutenFreeAppContext _context;

        public ConsumedDishesController(GlutenFreeAppContext context)
        {
            _context = context;
        }

        [HttpGet("userId")]
        public async Task<ActionResult<IEnumerable<ConsumedDish>>> GetDishesByUser(int userId)
        {
            return await _context.ConsumedDishes.Where(d => d.UserId == userId).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<ConsumedDish>> CreateConsumedDish(ConsumedDish dish)
        {
            _context.ConsumedDishes.Add(dish);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDishesByUser), new { userId = dish.UserId }, dish);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConsumedDish(int id, ConsumedDish dish)
        {
            if (id != dish.DishId)
                return BadRequest();

            _context.Entry(dish).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ConsumedDishes.Any(e => e.DishId == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsumedDish(int id)
        {
            var dish = await _context.ConsumedDishes.FindAsync(id);
            if (dish == null)
                return NotFound();

            _context.ConsumedDishes.Remove(dish);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
