using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _ctx;

        public AuthController(DataContext ctx)
        {
            _ctx = ctx;
        }

        [Route("Values")]
        public async Task<IActionResult> GetValues()
        {
            var values = await _ctx.Values.ToListAsync();
            return Ok(values);
        }

        [Route("Value/{id}")]
        public async Task<IActionResult> GetValue(int id)
        {
            var value = await _ctx.Values.FirstOrDefaultAsync(x=>x.Id == id);
            return Ok(value);
        }
    }
}