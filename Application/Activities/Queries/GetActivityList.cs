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

public class GetActivityList
{
    public class Query : IRequest<Result<PagedList<ActivityDto, DateTime?>>>
    {
        public required ActivityParams Params { get; set; }
    }
    public class Handler : IRequestHandler<Query, Result<PagedList<ActivityDto, DateTime?>>>
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
        public async Task<Result<PagedList<ActivityDto, DateTime?>>> Handle(Query request, CancellationToken cancellationToken)
        {
            DateTime compareDate = request.Params.Cursor ?? request.Params.StartDate;
            var query = _context.Activities.OrderBy(x => x.Date)
                .Where(x => x.Date >= compareDate)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Params.Filter))
            {
                query = request.Params.Filter switch
                {
                    "isGoing" => query.Where(x =>
                        x.Attendees.Any(a => a.UserId == _userAccessor.GetUserId())),
                    "isHost" => query.Where(x =>
                        x.Attendees.Any(a => a.IsHost && a.UserId == _userAccessor.GetUserId())),
                    _ => query
                };
            }

            var projectedActivities = query.ProjectTo<ActivityDto>(_mapper.ConfigurationProvider,
                    new { currentUserId = _userAccessor.GetUserId() });

            var activities = await projectedActivities.Take(request.Params.PageSize + 1)
                .ToListAsync(cancellationToken);
            DateTime? nextCursor = null;
            if (activities.Count > request.Params.PageSize)
            {
                nextCursor = activities.Last().Date;
                activities.RemoveAt(activities.Count - 1);
            }

            return Result<PagedList<ActivityDto, DateTime?>>.Success(
                new PagedList<ActivityDto, DateTime?>
                {
                    Items = activities,
                    NextCursor = nextCursor
                }
            );
        }
    }
}
