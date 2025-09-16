using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserManagementSystem.Domain.Models;

namespace UserManagementSystem.Domain.Repositorys
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IList<T>> GetAllAsync();
        Task CreateAsync(T model);
        Task<T?> GetByIdAsync(object id);
        void UpdateUser(T model);
        Task RemoveAsync(object id);
    }
}
