using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using UserManagementSystem.Domain;
using UserManagementSystem.Domain.Models;

namespace UserManagementSystem.Application.Features.Users.Queries
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, IList<User>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetUserQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IList<User>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
           return await _unitOfWork.UserRepository.GetAllAsync();
        }
    }
}
