using api_for_sambapos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_for_sambapos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemPropertyGroupsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MenuItemPropertyGroupsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItemPropertyGroups>>> GetMenuItemPropertyGroups()
        {
            try
            {
                var groupProperties = await _context.MenuItemPropertyGroups
                    .Include(g => g.Properties)
                    .AsNoTracking()
                    .Select(m => new MenuItemPropertyGroups
                    {
                        Id = m.Id,
                        Name = m.Name ?? string.Empty,
                        Order = m.Order ?? 0,
                        SingleSelection = m.SingleSelection,
                        MultipleSelection = m.MultipleSelection,
                        Properties = m.Properties ?? new List<MenuItemProperties>()
                    })
                    .OrderBy(x => x.Order)
                    .ToListAsync();

                return Ok(groupProperties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}