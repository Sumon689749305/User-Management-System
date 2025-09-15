using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using UserManagementSystem.Domain;
using UserManagementSystem.Domain.Models;

namespace UserManagementSystem.Application.Features.Authentication.Commands
{
    public class LoginCommandHandler: IRequestHandler<LoginCommand, String>
    {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public LoginCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<String> Handle(LoginCommand request, CancellationToken cancellationToken)
    {

            var user = _mapper.Map<LoginRequest>(request);
           var token= await _unitOfWork.LoginAsync(user);
            return token;
        }
}
}
