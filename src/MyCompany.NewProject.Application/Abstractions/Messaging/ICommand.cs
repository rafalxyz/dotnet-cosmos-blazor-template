using MyCompany.NewProject.Core.Results;
using MediatR;

namespace MyCompany.NewProject.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
