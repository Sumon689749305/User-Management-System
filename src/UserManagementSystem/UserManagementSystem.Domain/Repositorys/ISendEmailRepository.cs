using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementSystem.Domain.Repositorys
{
    public interface ISendEmailRepository
    {
        Task SendEmailAsync( string subject, string htmlMessage);
    }
}
