using Microsoft.AspNetCore.Http;

namespace HttpClientEnricher;

public class CorrelationIdMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("correlationId", out var correlationId))
        {
            CorrelationContext.SetCorrelationId(correlationId!);
        }
        else
        {
            CorrelationContext.SetCorrelationId(Guid.NewGuid().ToString());
        }

        await next(context);
    }
}