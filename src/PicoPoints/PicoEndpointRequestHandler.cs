using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace PicoPoints;

internal class PicoEndpointRequestHandler
{
    internal Expression BuildExpression()
    {
        var requestType = GetType().GenericTypeArguments[0];
        var requestParameter = Expression.Parameter(requestType, "request");
        var httpContextParameter = Expression.Parameter(typeof(HttpContext), "httpContext");
        var requestHandlerConstant = Expression.Constant(this, GetType());

        var processAsyncCall = Expression.Call(GetType().GetMethod(nameof(PicoEndpointRequestHandler<,>.ProcessAsync), )
    }
}

internal class PicoEndpointRequestHandler<TRequest, TResponse> : PicoEndpointRequestHandler
{
    private readonly Type _endpointType;
    
    internal string Route { get; set; }
    internal string Method { get; set; }

    internal PicoEndpointRequestHandler(PicoEndpoint picoEndpoint)
    {
        _endpointType = picoEndpoint.GetType();

        var configuration = picoEndpoint.Configure();
        Route = configuration.Route;
        Method = configuration.Method;
    }

    internal Task ProcessAsync(HttpContext httpContext, TRequest request)
    {
        var endpoint = (PicoEndpoint<TRequest, TResponse>) httpContext.RequestServices.GetRequiredService(_endpointType);

        return endpoint.ProcessAsync(httpContext, request);
    }
}
