using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RestaurantAPI.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private readonly ILogger _logger;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Stopwatch stop = new Stopwatch();

            stop.Start();

            await next.Invoke(context);

            stop.Stop();

            if(stop.Elapsed.TotalSeconds > 4)
            {
                _logger.LogWarning($"Execute time: {stop.Elapsed.TotalSeconds} from {context.Request.Method}");
            }
        }
    }
}
