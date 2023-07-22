using MyCompany.NewProject.Core.Results;
using MediatR;

namespace MyCompany.NewProject.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
