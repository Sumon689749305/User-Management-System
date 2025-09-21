using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementSystem.Domain;
using UserManagementSystem.Domain.Dtos;

namespace UserManagementSystem.Domain.Features.Users.Queries
{
    public interface IGetUserAndSearchQuery :IDataTables
    {
        public UserSearchDto SearchItem { get; set; }
    }
}
