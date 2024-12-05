using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HttpClientEnricher;

public static class CorrelationHttpClientExtension
{
    public static IServiceCollection AddCorrelationHttpClient(
        this IServiceCollection services,
        Action<HttpClient>? configureClient = null)
    {
        services.AddTransient<CorrelationIdHandler>();
        services.AddHttpClient("CorrelationHttpClient", client =>
            {
                //Configuração base, que poderia ser configurada dinamicamente
                client.BaseAddress = new Uri("http://localhost:5000");
                configureClient?.Invoke(client);
            })
            .AddHttpMessageHandler<CorrelationIdHandler>();
        return services;
    }

    public static HttpClient GetCorrelationHttpClient(this IHttpClientFactory factory)
    {
        return factory.CreateClient("CorrelationHttpClient");
    }
    
    public static IApplicationBuilder UseCorrelationIdMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
        return app;
    }
}