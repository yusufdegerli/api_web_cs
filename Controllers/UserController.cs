using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_for_sambapos.Models;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.Users
            .Include(u => u.UserRole)
            .ToListAsync();
    }

    [HttpGet("by-role/{roleId}")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsersByRole(int roleId)
    {
        return await _context.Users
            .Include(u => u.UserRole)
            .Where(u => u.UserRole_Id == roleId)
            .ToListAsync();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _context.Users
            .Include(u => u.UserRole)
            .FirstOrDefaultAsync(u => u.PinCode == loginDto.PinCode);

        if (user == null)
        {
            return Unauthorized("Geçersiz PIN kodu.");
        }

        if ( user.UserRole.Name != "Admin" && user.UserRole.Name != "Kasiyer" && user.UserRole.Name != "Garson")
        {
            return Unauthorized("Bu kullanıcı için yetki yok.");
        }

        return Ok(new
        {
            userId = user.Id,
            userName = user.Name,
            roleName = user.UserRole.Name,
            isAdmin = user.UserRole.IsAdmin
        });
    }

    public class  LoginDto
    {
        public required string PinCode { get; set; }
    }
}