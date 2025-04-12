using System;
using Application.Core;
using MediatR;
using Persistence;

namespace Application.Profiles.Commands;

public class SetMainPhoto
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string PhotoId {get; set;}
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly AppDbContext _context;
        private readonly IUserAccessor _userAccessor;
        public Handler (AppDbContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userAccessor.GetUserWithPhotosAsync();
            var photo = user.Photos.FirstOrDefault(x => x.Id == request.PhotoId);
            if (photo == null) return Result<Unit>.Failure("Cannot find photo", 400);
            user.ImageUrl = photo.Url;
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            return result ? Result<Unit>.Success(Unit.Value) 
                        : Result<Unit>.Failure("Ko update dc" , 400);
            
        }
    }
}
