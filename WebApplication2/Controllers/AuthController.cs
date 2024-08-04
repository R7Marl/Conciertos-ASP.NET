using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication2.Entity;
using WebApplication2.models;
using WebApplication2.Models;
using static AppDbContext;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("/auth")]
    public class AuthController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly UserService _userService;
        private IConfiguration _configuration;
        public AuthController(AppDbContext context, UserService userService, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _configuration = configuration;
        }
        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Register(UserModel user)
        {
            var existingUser = await _context.Users.Where(u => u.email == user.email).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return BadRequest("Usuario ya existe");
            }
            Users newUserEntity = new Users
            {
                name = user.name,
                email = user.email,
                role = string.IsNullOrEmpty(user.role) ? "user" : user.role,
                password = _userService.HashedPassword(user.password),
            };
            _context.Users.Add(newUserEntity);
            await _context.SaveChangesAsync();
            return Ok(newUserEntity);
        }
        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignIn(UserLogin loginData)
        {
            var userExist = await _context.Users.Where(u => u.email == loginData.Email).FirstOrDefaultAsync();
            if (userExist == null) {
                return BadRequest("Usuario no existe");
            }
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
    [Authorize]
    [HttpGet]
    [Route("data")]
    public string ProtectedRoute()
    {
        return "This is a protected route";
    }


    }
}
public class UserService : ServiceCollection
{

    public string HashedPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    public Boolean verifyPassword(string userPassword, string password) => BCrypt.Net.BCrypt.Verify(password, userPassword);

}