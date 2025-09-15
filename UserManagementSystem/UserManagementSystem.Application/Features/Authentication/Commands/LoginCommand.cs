using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace UserManagementSystem.Application.Features.Authentication.Commands
{
    public class LoginCommand:IRequest<string>
    {
        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
