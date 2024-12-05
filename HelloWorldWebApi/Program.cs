using HttpClientEnricher;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCorrelationHttpClient(client =>
{
    client.BaseAddress = new Uri("http://localhost:5082");
    client.DefaultRequestHeaders.Add("customHeader", Guid.NewGuid().ToString());
});
var app = builder.Build();
app.UseHttpsRedirection();
app.UseCorrelationIdMiddleware();

app.MapGet("/hello", async (IHttpClientFactory httpClientFactory) =>
{
    var httpClient = httpClientFactory.GetCorrelationHttpClient();
    var response = await httpClient.GetAsync("/headers");
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    return content;
}).WithName("Hello");

app.MapGet("/headers", (HttpContext httpContext) =>
{
    var headers = httpContext.Request.Headers;
    var headerList = headers.Select(h => $"{h.Key}: {h.Value}");
    return string.Join("\n", headerList);
}).WithName("Headers");

app.Run();