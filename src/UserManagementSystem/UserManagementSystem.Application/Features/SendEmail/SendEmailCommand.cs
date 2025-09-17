using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace UserManagementSystem.Application.Features.SendEmail
{
    public class SendEmailCommand : IRequest<string>
    {
        public string Subject { get; set; } 
        public string Message { get; set; }

    }
}
