using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace JwtImplementationDemo.Helper.Filters
{
    public class PerformanceMonitorFilter : IResultFilter
    {
        private readonly ILogger<PerformanceMonitorFilter> _logger;
        private Stopwatch _stopWatch;
        
        public PerformanceMonitorFilter(ILogger<PerformanceMonitorFilter> logger)
        {
            _logger = logger;
            _stopWatch = new Stopwatch();
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            _stopWatch = Stopwatch.StartNew();

            // metadata about the action being executed. -- ActionDescriptor
            var actionName = context.ActionDescriptor.DisplayName;

            _logger.LogInformation($"Starting execution of action: {actionName}"); 
        }


        public void OnResultExecuted(ResultExecutedContext context)
        {
            _stopWatch.Stop();

            var actionName = context.ActionDescriptor.DisplayName;
            var elapsedTime = _stopWatch.ElapsedMilliseconds;

            var statusCode = context.HttpContext.Response.StatusCode;

            _logger.LogInformation($"Action '{actionName}' executed in {elapsedTime} ms with status code {statusCode}");
        }

    }

}
