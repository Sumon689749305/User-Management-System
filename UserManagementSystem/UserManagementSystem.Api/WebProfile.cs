using AutoMapper;
using UserManagementSystem.Application.Features.Authentication.Commands;
using UserManagementSystem.Application.Features.Users.Commands;
using UserManagementSystem.Domain.Models;

namespace UserManagementSystem.Api
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<UserAddCommand, User>().ReverseMap();
            CreateMap<UserUpdateCommand, User>().ReverseMap();
            CreateMap<LoginCommand, LoginRequest>().ReverseMap();
        }
    }
}
