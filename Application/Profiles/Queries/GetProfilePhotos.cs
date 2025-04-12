using System;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles.Queries;

public class GetProfilePhotos
{
    public class Query : IRequest<Result<List<Photo>>>
    {
        public required string UserId {get; set;}
    }

    public class Handler : IRequestHandler<Query, Result<List<Photo>>>
    {
        private readonly AppDbContext _context;
        public Handler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<List<Photo>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var photos = await _context.Users.Where(x => x.Id == request.UserId)
                .SelectMany(x => x.Photos).ToListAsync(cancellationToken);
            return Result<List<Photo>>.Success(photos);
        }
    }

}
