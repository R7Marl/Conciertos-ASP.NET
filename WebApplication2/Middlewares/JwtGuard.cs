using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace WebApplication2.Middlewares
{
    public class JwtGuard : ControllerBase
    {
        private RequestDelegate _next;
        public JwtGuard(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if(token == null)
            {
                throw new ArgumentException("Token inválido");
            }
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("TuClaveSecretaAqui");
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                }, out SecurityToken validatedToken);

            }
            catch
            {
                throw new Exception("hubo un error");
                // Manejar la excepción en caso de que la validación del token falle.
            }

            await _next(context);
        }
    }
}
