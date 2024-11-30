using MediatR;

namespace BuildingBlocks.CQRS;

/// <summary>
/// Interface used when the command handler does not return a response.
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit>
    where TCommand : ICommand<Unit> { }

/// <summary>
/// Interface used when the command handler does return a response.
/// </summary>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : notnull { }
