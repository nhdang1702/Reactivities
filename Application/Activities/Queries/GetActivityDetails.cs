using System;
using Application.Activities.DTOs;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Queries;

public class GetActivityDetails
{
    public class Query : IRequest<Result<ActivityDto>>
    {
        public required string Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<ActivityDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;
        public Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor)
        {
            _context = context;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }
        public async Task<Result<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider, new {currentUserId = _userAccessor.GetUserId()})
                .FirstOrDefaultAsync(x => request.Id == x.Id, cancellationToken);
            if (activity == null)
            {
                return Result<ActivityDto>.Failure("Activity not found!", 404);
            }
            return Result<ActivityDto>.Success(_mapper.Map<ActivityDto>(activity));
        }
    }

}
