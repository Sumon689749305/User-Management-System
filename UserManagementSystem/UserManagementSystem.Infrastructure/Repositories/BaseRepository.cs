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
            IQueryable<T> query = _context.Set<T>();
            return await query.ToListAsync();
        }
        public async Task<T?> GetByIdAsync(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void UpdateUser(T entiey)
        {
            _context.Set<T>().Update(entiey);
        }
        public async Task RemoveAsync(object id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
    
            _context.Set<T>().Remove(entity);
        }
    }
}
