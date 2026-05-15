namespace RestaurantOrders.Application.Common.Behaviors;

using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var nome = typeof(TRequest).Name;
        logger.LogInformation("Executando {Request}", nome);

        var sw       = Stopwatch.StartNew();
        var response = await next();
        sw.Stop();

        logger.LogInformation("{Request} concluído em {Ms}ms", nome, sw.ElapsedMilliseconds);
        return response;
    }
}
