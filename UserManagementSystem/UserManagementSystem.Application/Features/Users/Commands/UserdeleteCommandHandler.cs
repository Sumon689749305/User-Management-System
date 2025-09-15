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
    public class UserdeleteCommandHandler :IRequestHandler<UserDeleteCommand, User>
    {
        private readonly IUnitOfWork _unitOfWork;
    public UserdeleteCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
       
    }
    public async Task<User> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with Id {request.Id} not found.");
            }
            await _unitOfWork.UserRepository.RemoveAsync(user.Id);
        await _unitOfWork.SaveAsync();
     return user;
    }
}
}
