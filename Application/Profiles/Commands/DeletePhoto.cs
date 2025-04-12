using System;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Persistence;

namespace Application.Profiles.Commands;

public class DeletePhoto
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string PhotoId {get; set;}
    }
    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly AppDbContext _context;
        private readonly IPhotoService _photoService;
        public Handler(IUserAccessor userAccessor, AppDbContext context, IPhotoService photoService)
        {
            _userAccessor = userAccessor;
            _context = context;
            _photoService = photoService;
        }
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userAccessor.GetUserWithPhotosAsync();

            var photo = user.Photos.FirstOrDefault(x => x.Id == request.PhotoId);

            if (photo == null) return Result<Unit>.Failure("Cannot find the photo", 400);

            if (photo.Url == user.ImageUrl) return Result<Unit>.Failure("Cannot delete main photo", 400);
            
            await _photoService.DeletePhoto(photo.PublicId);
            
            // if (deleteResult.Error != null)
            // {
            //     return Result<Unit>.Failure(deleteResult.Error.)
            // }
            user.Photos.Remove(photo);
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            return result
                ? Result<Unit>.Success(Unit.Value)
                : Result<Unit>.Failure("Problem deleting photo", 400);
        }
    }

}

public interface IRequestHandler<T1, T2, T3>
{
}