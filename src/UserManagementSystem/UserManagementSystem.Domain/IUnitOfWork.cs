using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementSystem.Domain.Dtos;
using UserManagementSystem.Domain.Models;
using UserManagementSystem.Domain.Repositorys;

namespace UserManagementSystem.Domain
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        Task SaveAsync();
        Task<string?> LoginAsync(LoginRequest loginrequest);
        Task<(IList<User> data, int total, int totalDisplay)> GetUsersSP(int pageIndex,
            int pageSize, string? order, UserSearchDto request);
    }
}
