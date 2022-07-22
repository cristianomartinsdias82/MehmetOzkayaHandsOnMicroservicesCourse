using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Helpers.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Behaviours
{
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger ?? throw new ArgumentNullException($"Argument {nameof(logger)} cannot be null.");
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next(); // >>> THIS IS THE COMMAND/QUERY HANDLER BEING EXECUTED!
            }
            catch (Exception exc)
            {
                UnobtrusiveLoggingHelper.Log(
                    _logger,
                    "An unhandled exception for request {Name} {@Request} has occurred.",
                    LogLevel.Error,
                    exc,
                    args: new object[] { typeof(TRequest), request });

                throw;
            }
        }
    }
}
