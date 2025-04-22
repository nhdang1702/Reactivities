using System;
using Application.Core;
using Application.Profiles.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles.Queries;

public class GetUserActivities
{
    public class Query : IRequest<Result<List<UserActivityDto>>>
    {
        public required string UserId { get; set; }
        public required string Filter { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<List<UserActivityDto>>>
    {
        private readonly AppDbContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;
        public Handler(AppDbContext context, IUserAccessor userAccessor, IMapper mapper)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mapper = mapper;
        }

        public async Task<Result<List<UserActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _context.ActivityAttendees
                .Where(x => x.User.Id == request.UserId)
                .OrderBy(x => x.Activity.Date)
                .Select(x => x.Activity)
                .AsQueryable();

            var today = DateTime.UtcNow;

            query = request.Filter switch
            {
                "past" => query.Where(a => a.Date <= today
                    && a.Attendees.Any(x => x.UserId == request.UserId)),
                "hosting" => query.Where(a => a.Attendees.Any(x => x.IsHost
                    && x.UserId == request.UserId)),
                _ => query.Where(a => a.Date >= today
                    && a.Attendees.Any(x => x.UserId == request.UserId))
            };

            var projectedActivities = query.ProjectTo<UserActivityDto>(_mapper.ConfigurationProvider);

            var activities = await projectedActivities.ToListAsync(cancellationToken);

            return Result<List<UserActivityDto>>.Success(activities);
        }
    }

}
