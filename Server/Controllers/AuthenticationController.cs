using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Reponsitories.Contracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IUserAccount accountInterface) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<IActionResult> createAsync(Register user)
        {
            if (user == null) return BadRequest("Model is empty");
            var results = await accountInterface.CreateAsync(user);
            return Ok(results);

        }

        [HttpPost("Login")]
        public async Task<IActionResult> SignIn(Login user)
        {
            if (user == null) return BadRequest("Model is empty");
            var results = await accountInterface.SignInAsync(user);
            return Ok(results);

        }
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshToken user)
        {
            if (user == null) return BadRequest("Model is empty");
            var results = await accountInterface.RefreshTokenAsync(user);
            return Ok(results);

        }

    }
}
