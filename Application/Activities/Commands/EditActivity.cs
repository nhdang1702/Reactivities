using System;
using Domain;
using MediatR;
using Persistence;
using AutoMapper;

namespace Application.Activities.Commands;

public class EditActivity
{
    public class Command : IRequest
    {
        public required Activity Activity { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public Handler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities.FindAsync(request.Activity.Id, cancellationToken);
            if (activity == null)
            {
                throw new Exception("Activity not found");
            }
            _mapper.Map(request.Activity, activity);
            await _context.SaveChangesAsync(cancellationToken);

        
        }
    }
}
