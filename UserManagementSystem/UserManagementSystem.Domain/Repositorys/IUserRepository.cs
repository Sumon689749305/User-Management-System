using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementSystem.Domain.Models;

namespace UserManagementSystem.Domain.Repositorys
{
    public interface IUserRepository:IBaseRepository<User>
    {
    }
}
