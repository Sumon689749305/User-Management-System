using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserManagementSystem.Domain.Models;
using UserManagementSystem.Domain.Repositorys;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace UserManagementSystem.Infrastructure.Repositories
{
    public class BaseRepository<T> :IBaseRepository<T> where T : class
    {

        private readonly UserManagementSystemContext _context;
       
        public BaseRepository(UserManagementSystemContext context)
        {
            _context = context;
           
        }

        public async Task<IList<T>> GetAllAsync()
        {
            var result = await _context.Set<T>()
             .FromSqlRaw($"EXEC sp_GetAll{typeof(T).Name}s")
             .ToListAsync();
            return result;
        }
        public async Task<T?> GetByIdAsync(object id)
        {
            var result = _context.Set<T>()
        .FromSqlRaw($"EXEC sp_Get{typeof(T).Name}ById @Id = {{0}}", id)
        .AsEnumerable()   
        .FirstOrDefault(); 

            return result;
        }
        public async Task CreateAsync(T entity)
        {
            var name = entity.GetType().GetProperty("Name")?.GetValue(entity);
            var userName = entity.GetType().GetProperty("UserName")?.GetValue(entity);
            var password = entity.GetType().GetProperty("Password")?.GetValue(entity);

            await _context.Database.ExecuteSqlRawAsync(
               $"EXEC sp_Add{typeof(T).Name}s @p0, @p1, @p2",
                  name, userName, password);
        }
        public async Task UpdateUser(T entity)
        {
            var name = entity.GetType().GetProperty("Name")?.GetValue(entity);
            var userName = entity.GetType().GetProperty("UserName")?.GetValue(entity);
            var password = entity.GetType().GetProperty("Password")?.GetValue(entity);
            var id = entity.GetType().GetProperty("Id")?.GetValue(entity);

             await _context.Database.ExecuteSqlRawAsync(
                $"EXEC sp_Update{typeof(T).Name}s @Id = {{0}}, @Name = {{1}}, @UserName = {{2}}, @Password = {{3}}",
                id, name, userName, password);
        }
        public async Task RemoveAsync(object id)
        {
            await _context.Database.ExecuteSqlRawAsync(
        $"EXEC sp_Delete{typeof(T).Name}s @Id = {{0}}", id);
        }
    }
}
