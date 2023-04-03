using System;
using MediatR;

namespace API.Application.Commands
{
    /**
    Generic request class to get entities. Takes id as a param.
    **/
    public class GetByIdCommand<T> : IRequest<T>
    {
        public Guid Id { get; private set; }

        public GetByIdCommand(Guid id)
        {
            Id = id;
        }
    }
}
