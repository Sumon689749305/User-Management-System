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
    public class GetUserByIdHandler:IRequestHandler<GetUserById, User>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public async Task<User> Handle(GetUserById request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.UserRepository.GetByIdAsync(request.Id);

        }
    }
}
