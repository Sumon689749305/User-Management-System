using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using UserManagementSystem.Domain;
using UserManagementSystem.Domain.Dtos;
using UserManagementSystem.Domain.Models;

namespace UserManagementSystem.Application.Features.Users.Queries
{
    public class GetUserAndSearchQueryHandler :IRequestHandler<GetUserAndSearchQuery,(IList<User>,int,int)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetUserAndSearchQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<(IList<User>, int, int)> Handle(GetUserAndSearchQuery request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<UserSearchDto>(request.SearchItem);
            return await _unitOfWork.GetUsersSP(request.PageIndex, request.PageSize, 
                request.FormatSortExpression("Name","UserName","Password","Id"), user);
        }
    }
}
