using MyCompany.NewProject.Core.Results;
using MediatR;

namespace MyCompany.NewProject.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}