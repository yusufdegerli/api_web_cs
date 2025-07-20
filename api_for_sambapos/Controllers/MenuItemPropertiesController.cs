using api_for_sambapos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_for_sambapos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemPropertiesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MenuItemPropertiesController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItemProperties>>> GetMenuItemProperties()
        {
            try
            {
                var properties = await _context.MenuItemProperties
                    .Include(m => m.MenuItemPropertyGroup) // 💥 BURAYI EKLE
                    .AsNoTracking()
                    .Select(m => new MenuItemProperties
                    {
                        Id = m.Id,
                        Name = m.Name ?? string.Empty,
                        MenuItemPropertyGroupId = m.MenuItemPropertyGroupId,
                        MenuItemPropertyGroup = m.MenuItemPropertyGroup != null
                            ? new MenuItemPropertyGroups
                            {
                                Id = m.MenuItemPropertyGroup.Id,
                                Name = m.MenuItemPropertyGroup.Name ?? string.Empty,
                                Order = m.MenuItemPropertyGroup.Order,
                                SingleSelection = m.MenuItemPropertyGroup.SingleSelection,
                                MultipleSelection = m.MenuItemPropertyGroup.MultipleSelection
                            }
                            : null
                    })
                    .ToListAsync();

                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
