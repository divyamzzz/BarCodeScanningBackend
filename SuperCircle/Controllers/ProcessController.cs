using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperCircle.Models;

namespace SuperCircle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessController : ControllerBase
    {
        private readonly SuperCircleContext _context;
        public ProcessController(SuperCircleContext context)
        {
            _context = context;
        }

        [HttpGet("GetProcessData")]
        public async Task<ActionResult> GetProcessData()
        {
            try
            {
                var process = await _context.ProcessMasters.AsNoTracking().ToListAsync();
                return Ok(process);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
    }

   
}
