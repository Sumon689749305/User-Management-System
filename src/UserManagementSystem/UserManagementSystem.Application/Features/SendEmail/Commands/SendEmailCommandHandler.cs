using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using MediatR;
using UserManagementSystem.Domain.Repositorys;

namespace UserManagementSystem.Application.Features.SendEmail.Commands
{
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, string>
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly ISendEmailRepository _sendEmailRepository;

        public SendEmailCommandHandler(IBackgroundJobClient backgroundJobClient,ISendEmailRepository sendEmailRepository)
        {
            _backgroundJobClient = backgroundJobClient;
            _sendEmailRepository = sendEmailRepository;
        }

        public Task<string> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            _backgroundJobClient.Enqueue(() =>
            _sendEmailRepository.SendEmailAsync(request.Subject, request.Message));

            return Task.FromResult("Email job has been queued successfully.");
        }
    }
}
