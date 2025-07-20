using api_for_sambapos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace api_for_sambapos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemPortionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MenuItemPortionsController(AppDbContext context) => _context = context;
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItemPortions>>> GetMenuPortions()
        {
            try
            {
                var portions = await _context.MenuItemPortions
                    .AsNoTracking()
                    .Select(m => new MenuItemPortions
                    {
                        Id = m.Id,
                        Name = m.Name,
                        MenuItemId = m.MenuItemId,
                        Price_Amount = m.Price_Amount
                    }).ToListAsync();

                return Ok(portions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
