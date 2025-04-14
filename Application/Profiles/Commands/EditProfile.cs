using System;
using System.Data.Common;
using Application.Core;
using Application.Profiles.DTOs;
using MediatR;
using Persistence;

namespace Application.Profiles.Commands;

public class EditProfile
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string DisplayName{get; set;}
        public required string Bio {get; set;}
    }
    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly AppDbContext _context;
        public Handler(IUserAccessor userAccessor, AppDbContext context)
        {
            _userAccessor = userAccessor;
            _context = context;
        }
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userAccessor.GetUserAsync();
            if (user == null) 
            {
                return Result<Unit>.Failure("User not found!", 404);
            }

            user.DisplayName = request.DisplayName;
            user.Bio = request.Bio;
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;
            return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Failed to edit profile", 400);
        }
    }
}
