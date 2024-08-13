using ConciertosNET.Services;
using ConciertosNET.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication2.Entity;
using WebApplication2.models;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("/auth")]

    public class AuthController : ControllerBase
    {
        UserService _userService;
        IConfiguration _configuration;

        public AuthController(UserService userService, IConfiguration configuration)
        {
            this._userService = userService;
            this._configuration = configuration;
        }
        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Register(UserModel user)
        {
            try
            {
                var existingUser = await _userService.GetUserByEmail(user.email);
                if (existingUser != null) return StatusCode(400, "User already exist");
                var newUserEntity = new Users
                {
                    name = user.name,
                    email = user.email,
                    role = string.IsNullOrEmpty(user.role) ? "user" : user.role,
                    password = _userService.HashedPassword(user.password),
                };
                await _userService.CreateUser(newUserEntity);
                return Ok(newUserEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignIn(UserLogin loginData)
        {
            try
            {
                Users userExist = await _userService.GetUserByEmail(loginData.Email);
                if (userExist == null) return StatusCode(400, "El usuario no existe"); 
                Boolean PasswordCompare = _userService.verifyPassword(userExist.password, loginData.Password);
                if (PasswordCompare)
                {
                    var jwt = _configuration["Jwt:Key"]!;
                    var payload = new[]
                    {
                    new Claim("email", userExist.email),
                    new Claim("role", userExist.role),
                    new Claim("id", userExist.id.ToString())
                };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(claims: payload, expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: signIn);
                    return Ok(new
                    {
                        access_token = new JwtSecurityTokenHandler().WriteToken(token),
                        result = userExist
                    });
                }

                return BadRequest("Las contraseñas no coinciden");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    [Authorize]
    [HttpGet]
    [Route("data")]
    public string ProtectedRoute()
    {
        return "This is a protected route";
    }


    }
}