using AutoMapper;
using UserManagementSystem.Application.Features.Authentication.Commands;
using UserManagementSystem.Application.Features.Users.Commands;
using UserManagementSystem.Application.Features.Users.Queries;
using UserManagementSystem.Domain.Dtos;
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

            CreateMap<GetUserAndSearchQuery, UserSearchDto>().ReverseMap();
        }
    }
}
