using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using UserManagementSystem.Domain;
using UserManagementSystem.Domain.Models;

namespace UserManagementSystem.Application.Features.Users.Commands
{
    public class UserUpdateCommandHandler : IRequestHandler<UserUpdateCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserUpdateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task Handle(UserUpdateCommand request, CancellationToken cancellationToken)
        {

           var user = _mapper.Map<User>(request);

             _unitOfWork.UserRepository.UpdateUser(user);
            await _unitOfWork.SaveAsync();

        }
        }
    }
