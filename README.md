# POC Custom HttpClient

Essa POC tem como objetivo validar a possibilidade de disponibilização de um `HttpClient` customizado através de uma `classlib` para uso em aplicações.

## Implementações

- `CorrelationContext`: Armazena o um id de correlação no contexto de execução
- `CorrelationIdHandler`: Handler adicionado por padrão no `HttpClient` customizado para obter o id de correlação do contexto
- `CorrelationIdMiddleware`: Middleware para obter o id de correlação da requisição original e criar, caso não exista.
- `CorrelationHttpClientExtension`: Extensões para fornecer o `HttpClient` customizado e o `Middleware`

## Configuração na aplicação

- Obtém o `HttpClient` customizado e permite alterar e adicionar novas configurações.

```csharp
builder.Services.AddCorrelationHttpClient(client =>
{
    client.BaseAddress = new Uri("http://localhost:5082");
    client.DefaultRequestHeaders.Add("customHeader", Guid.NewGuid().ToString());
});
```
- Adiciona o `Middleware` para captura das informações e gravação no contexto

```csharp
var app = builder.Build();
app.UseCorrelationIdMiddleware();
```

- Obtenção do `HttpClient` e execução de requisição:

```csharp
var httpClient = httpClientFactory.GetCorrelationHttpClient();
var response = await httpClient.GetAsync("/headers");
```

## Testando

- Chamada sem a passagem do id de correlação: `curl localhost:5082/hello`

Exemplo de resultado, onde o `correlationId` recebido foi criado aleatoriamente:

```shell
Host: localhost:5082
traceparent: 00-028d33d3beca16281240c728d2167b27-4ab5bd2185ddbbc3-00
customHeader: 3dc02119-a506-4ede-9cef-d775400834ab
correlationId: 9cfe869f-b506-480f-bbdc-4093b73a3613
```

- Chamada com a passagem do id de correlação: `curl -H "correlationId: 123456" localhost:5082/hello`

Exemplo de resultado, onde o `correlationId` recebido é o mesmo da requisição original:

```shell
Host: localhost:5082
traceparent: 00-845c10d4791dc297e3a0a31d70738fbe-5f71edd57bc5743c-00
customHeader: b89cb506-07e3-40d2-8dc0-d5e341f2a714
correlationId: 123456
```
