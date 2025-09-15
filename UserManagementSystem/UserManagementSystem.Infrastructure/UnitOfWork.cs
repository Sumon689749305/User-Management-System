using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserManagementSystem.Domain;
using UserManagementSystem.Domain.Models;
using UserManagementSystem.Domain.Repositorys;
using UserManagementSystem.Infrastructure.Repositories;

namespace UserManagementSystem.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserManagementSystemContext _dbContext;
        private readonly IConfiguration _configuration;
        public UnitOfWork(UserManagementSystemContext dbContext,IUserRepository userRepository,IConfiguration configuration)
        {
            _dbContext = dbContext;
            UserRepository = userRepository;
            _configuration = configuration;
        }
        public IUserRepository UserRepository { get; private set; }
        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<string?> LoginAsync(LoginRequest loginrequest)
        {
            if (!string.IsNullOrEmpty(loginrequest.UserName) && !string.IsNullOrEmpty(loginrequest.Password))
            {
                var user = await _dbContext.Users
                    .SingleOrDefaultAsync(s => s.UserName == loginrequest.UserName && s.Password == loginrequest.Password);

                if (user != null)
                {
                    var claims = new List<Claim>
                {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim("Id", user.Id.ToString()),
                new Claim("Name", user.Name)
                 };

                    var userRoles = await _dbContext.UserRoles.Where(u => u.UserId == user.Id).ToListAsync();
                    var roleIds = userRoles.Select(s => s.RoleId).ToList();

                    var roles = await _dbContext.Roles.Where(r => roleIds.Contains(r.Id)).ToListAsync();
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Name));
                    }

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],
                        audience: _configuration["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        signingCredentials: signIn);

                    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                    return jwtToken;
                }
                else
                {
                    throw new Exception("user is not valid");
                }

            }
            else
            {
                throw new Exception("creadentials are not valid");
            }

        }
    }
}
