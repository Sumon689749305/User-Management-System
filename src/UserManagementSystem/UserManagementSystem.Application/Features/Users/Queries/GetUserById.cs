using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using UserManagementSystem.Domain.Models;

namespace UserManagementSystem.Application.Features.Users.Queries
{
    public class GetUserById : IRequest<User>
    {
        public int Id { get; }

        public GetUserById(int id)
        {
            Id = id;
        }
    }
}
