namespace HttpClientEnricher;

public class CorrelationIdHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var correlationId = CorrelationContext.GetCorrelationId();

        if (!string.IsNullOrEmpty(correlationId))
        {
            request.Headers.Add("correlationId", correlationId);
        }
        else
        {
            request.Headers.Add("correlationId", Guid.NewGuid().ToString());
        }

        return await base.SendAsync(request, cancellationToken);
    }
}