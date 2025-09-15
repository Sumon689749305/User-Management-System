using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using UserManagementSystem.Domain;
using UserManagementSystem.Domain.Models;
using UserManagementSystem.Infrastructure;

namespace UserManagementSystem.Application.Features.Users.Commands
{
    public class UserAddCommandHandler : IRequestHandler<UserAddCommand,int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserAddCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<int> Handle(UserAddCommand request, CancellationToken cancellationToken)
        {

            var user = _mapper.Map<User>(request);

            await _unitOfWork.UserRepository.CreateAsync(user);
            await _unitOfWork.SaveAsync();

            return user.Id;
        }

    }
}
