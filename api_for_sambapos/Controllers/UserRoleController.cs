using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_for_sambapos.Models;

namespace api_for_sambapos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserRoleController
    {
        private readonly AppDbContext _context;

        public UserRoleController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRoles>>> GetUserRoles()
        {
            return await _context.UserRoles.ToListAsync();
        }
    }
}
