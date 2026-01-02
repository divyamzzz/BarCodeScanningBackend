using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SuperCircle.Models;
using System.Runtime.CompilerServices;

namespace SuperCircle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly SuperCircleContext _context; 
        public LoginController(SuperCircleContext context)
        {
            _context = context;
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult>CreateUser(UserLoginDto dto)
        {
            try
            {
                var hasher = new PasswordHasher<string>();
                string hashPassword = hasher.HashPassword(null, dto.Password);
                var user = new UserLogin
                {
                    UserName = dto.UserName,
                    Email = dto.Email,
                    PasswordHash = hashPassword,
                    FullName = dto.Name,
                    IsActive = true,
                };
                //
                _context.UserLogins.Add(user);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult>Login(LoginDto dto)
        {
            try
            {
                if (dto.userName == null)
                {
                    if (dto.email == null)
                    {
                        return BadRequest("Email or Username Does Not Exsist");
                    }
                    else
                    {
                        //
                    }

                }
                var hasher = new PasswordHasher<string>();
                var user = await _context.UserLogins.Where(u => u.UserName == dto.userName||u.Email==dto.email).FirstOrDefaultAsync();
                
                var password = hasher.VerifyHashedPassword(null, user.PasswordHash, dto.password);
                if (password == PasswordVerificationResult.Success)
                {
                    return Ok(user);
                }
                return BadRequest("Login Failed");

            }
            catch (Exception ex) {
                return BadRequest(ex.Message);

            }
        }
        [HttpPost("LoginWithEmail")]
        public async Task<ActionResult>LoginWithEmail(LoginWithEmailDto dto)
        {
            try
            {
                if (dto.email == null)
                {
                    return BadRequest("Email Does Not Exsist");

                }
                var hasher = new PasswordHasher<string>();
                var user = await _context.UserLogins.Where(u => u.UserName == dto.email).FirstOrDefaultAsync();
                var password = hasher.VerifyHashedPassword(null, user.PasswordHash, dto.password);
                if (password == PasswordVerificationResult.Success)
                {
                    return Ok(user);
                }
                return BadRequest("Login Failed");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        public class UserLoginDto
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public string Name { get; set;}

        }

        public class LoginDto
        {
            public string? userName { get; set; }
            public string password { get; set; }
            public string ? email { get; set; }
        }
        public class LoginWithEmailDto
        {
            public string email { get; set; }
            public string password { get; set; }
        }
    }
}
