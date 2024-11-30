using MediatR;

namespace BuildingBlocks.CQRS;

/// <summary>
/// Interface used when the query handler does return a response.
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : notnull { }
