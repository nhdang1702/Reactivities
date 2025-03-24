using System;
using Application.Activities.DTOs;
using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities.Commands;

public class CreateActivity
{
    public class Command : IRequest<Result<string>>
    {
       public required CreateActivityDto ActivityDto { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<string>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public Handler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = _mapper.Map<Activity>(request.ActivityDto);
            _context.Activities.Add(activity);
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (!result) return Result<string>.Failure("Failed to create the activity", 400);
            return Result<string>.Success(activity.Id);
        }
    }

}
