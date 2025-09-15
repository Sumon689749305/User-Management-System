using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementSystem.Domain.Models;
using UserManagementSystem.Domain.Repositorys;

namespace UserManagementSystem.Infrastructure.Repositories
{
    public class UserRepository:BaseRepository<User>,IUserRepository
    {
        private readonly UserManagementSystemContext _context;

        public UserRepository(UserManagementSystemContext context) : base(context)
        {
            _context = context;
        }

    }
}
