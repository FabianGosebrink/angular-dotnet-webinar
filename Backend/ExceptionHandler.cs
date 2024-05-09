using Microsoft.AspNetCore.Diagnostics;

namespace ExpenseTracker;

/**
 * Browser           Middleware           Service
   |                 |                  |
   |---Request------>|                  |
   |                 |---Request------->|
   |                 |<--Response-------|
   |                 X                  X
   |<--Response------|                  |
   |                 |                  |
   |---Request------>|                  |
   |                 |---Request------->|
   |                 |<--Exception------|
   |<--Error Object--|                  |
 **/
public class ExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ExceptionHandler> _logger;

    public ExceptionHandler(ILogger<ExceptionHandler> logger)
    {
        _logger = logger;
    }
    
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "We encountered an unhanded exception");

        return ValueTask.FromResult(true);
    }
}