using JwtImplementationDemo.Helper.Exceptions;
using JwtImplementationDemo.Dto;
using JwtImplementationDemo.Entities;
using JwtImplementationDemo.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtImplementationDemo.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _configuration;

        private readonly ParcticeContext _context;


        public AuthRepository(IConfiguration configuration, ParcticeContext context)
        {
            _configuration = configuration;
            _context = context;
        }



        public async Task<ResponseModel> Login(LoginModel model)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync( x => x.UserName == model.UserName && x.Password == model.Password);

            if (user == null) 
            {
                throw new UnauthorizedException("Invalid credentials.");
            }

            return new ResponseModel
            {
                Message = "Token generated successfully.",
                Result = GenerateToken(user)
            };
        }




        private string GenerateToken(User user)
        {

            // create claims for JWT
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role,user.Role),
                new Claim("UserId", user.UserId.ToString()),
            };


            // Get secret key from configuration
            var jwtKey = _configuration["JWT:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT:Key is missing in configuration");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // generate JWT
            var token = new JwtSecurityToken(
                issuer:_configuration["JWT:Issuer"],
                audience:_configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credential);


            // Return token as string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
