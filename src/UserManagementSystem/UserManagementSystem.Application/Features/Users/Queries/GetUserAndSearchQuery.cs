using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementSystem.Domain;
using MediatR;
using UserManagementSystem.Domain.Dtos;
using UserManagementSystem.Domain.Features.Users.Queries;
using UserManagementSystem.Domain.Models;

namespace UserManagementSystem.Application.Features.Users.Queries
{
    public class GetUserAndSearchQuery : DataTables,IRequest<(IList<User>,int, int)>,IGetUserAndSearchQuery
    {
        public UserSearchDto SearchItem { get; set; }
    }
}
