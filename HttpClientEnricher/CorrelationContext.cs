namespace HttpClientEnricher;

public static class CorrelationContext
{
    private static readonly AsyncLocal<string> CorrelationId = new();

    public static string? GetCorrelationId() => CorrelationId.Value;

    public static void SetCorrelationId(string correlationId)
    {
        CorrelationId.Value = correlationId;
    }
}