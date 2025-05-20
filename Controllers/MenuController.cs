using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_for_sambapos.Models;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly AppDbContext _context;

    public MenuController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenuItems()
    {
        try
        {
            var menuItems = await _context.MenuItems
                .Include(x => x.Portions)
                .AsNoTracking()
                .Select(m => new MenuItem
                {
                    Id = m.Id,
                    Name = m.Name ?? string.Empty,
                    GroupCode = m.GroupCode ?? string.Empty,
                    Portions = m.Portions ?? new List<MenuItemPortions>(),
                    Price = (m.Portions != null && m.Portions.Any())
                        ? m.Portions.First().Price_Amount
                        : 0,
                    Category = m.GroupCode ?? "Main"
                })
                .ToListAsync();

            return Ok(menuItems);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("by-category/{category}")]
    public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenuItemsByCategory(string category)
    {
        try
        {
            var menuItems = await _context.MenuItems
                .Include(x => x.Portions)
                .Where(m => m.GroupCode == category)
                .AsNoTracking()
                .Select(m => new MenuItem
                {
                    Id = m.Id,
                    Name = m.Name ?? string.Empty,
                    GroupCode = m.GroupCode ?? string.Empty,
                    Portions = m.Portions ?? new List<MenuItemPortions>(),
                    Price = (m.Portions != null && m.Portions.Any())
                        ? m.Portions.First().Price_Amount
                        : 0,
                    Category = m.GroupCode ?? "Main"
                })
                .ToListAsync();

            return Ok(menuItems);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("items/{id}/property-groups")]
    public async Task<ActionResult<IEnumerable<MenuItemPropertyGroups>>> GetPropertyGroupsForMenuItem(int id)
    {
        try
        {
            var propertyGroups = await _context.MenuItemProperties
                .Where(p => p.MenuItemId == id && p.MenuItemPropertyGroupId != null)
                .Join(
                    _context.MenuItemPropertyGroups.Include(g => g.Properties),
                    p => p.MenuItemPropertyGroupId,
                    g => g.Id,
                    (p, g) => new MenuItemPropertyGroups
                    {
                        Id = g.Id,
                        Name = g.Name ?? string.Empty,
                        Order = g.Order ?? 0,
                        SingleSelection = g.SingleSelection,
                        MultipleSelection = g.MultipleSelection,
                        Properties = g.Properties ?? new List<MenuItemProperties>()
                    })
                .OrderBy(g => g.Order)
                .ToListAsync();

            return Ok(propertyGroups);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("items/{id}/properties")]
    public async Task<ActionResult<IEnumerable<MenuItemProperties>>> GetItemProperties(int id)
    {
        try
        {
            var properties = await _context.MenuItemProperties
                .Where(p => p.MenuItemId == id)
                .Include(p => p.MenuItemPropertyGroup)
                .ToListAsync();

            return Ok(properties);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}