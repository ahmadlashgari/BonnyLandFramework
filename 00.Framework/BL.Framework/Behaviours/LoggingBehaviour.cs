using BL.Framework.AspNetCore.Security;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Framework.Behaviours
{
	public class LoggingBehaviour<TRequest, TResponse> : IRequestPreProcessor<TRequest> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;
        private readonly ICurrentUserService _currentUserService;

        public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserId ?? string.Empty;

            _logger.LogInformation("BL Framework Request: {Name} {@UserId} {@Request}",
                requestName, userId, request);
        }
    }
}
