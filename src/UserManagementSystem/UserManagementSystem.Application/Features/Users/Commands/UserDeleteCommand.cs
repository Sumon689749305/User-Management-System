using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using UserManagementSystem.Domain.Models;

namespace UserManagementSystem.Application.Features.Users.Commands
{
    public class UserDeleteCommand :IRequest<User>
    {
        public int Id { get; set; }

        public UserDeleteCommand(int id)
        {
            Id = id;
        }
    }
}
