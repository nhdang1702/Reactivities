using System;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Queries;

public class GetActivityDetails
{
    public class Query : IRequest<Activity>
    {
        public required string Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Activity>
    {
        private readonly AppDbContext _context;
        public Handler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Activity> Handle(Query request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities.FindAsync(request.Id, cancellationToken);
            if (activity == null)
            {
                throw new Exception("Activity not found");
            }
            return activity;
        }
    }

}
