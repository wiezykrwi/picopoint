using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace PicoPoints;

internal class PicoEndpointRequestHandler
{
    public readonly Type EndpointType;

    protected PicoEndpointRequestHandler(Type endpointType)
    {
        EndpointType = endpointType;
    }

    internal Delegate BuildExpression()
    {
        var requestHandlerType = GetType();
        var requestType = requestHandlerType.GenericTypeArguments[0];
        var requestParameter = Expression.Parameter(requestType, "request");
        var httpContextParameter = Expression.Parameter(typeof(HttpContext), "httpContext");
        var requestHandlerConstant = Expression.Constant(this, requestHandlerType);

        var types = new[] { typeof(HttpContext), requestType };
        var processMethod = requestHandlerType.GetMethod("ProcessAsync", types) ??
                            throw new Exception("Internal error - ProcessAsync could not be found");
        var processAsyncCall = Expression.Call(requestHandlerConstant, processMethod, httpContextParameter, requestParameter);

        return Expression.Lambda(processAsyncCall, httpContextParameter, requestParameter).Compile();
    }

    internal PicoEndpointConfiguration GetConfiguration(IServiceProvider serviceProvider)
    {
        var picoEndpoint = (PicoEndpoint) serviceProvider.GetRequiredService(EndpointType);

        return picoEndpoint.Configure();
    }
}

internal class PicoEndpointRequestHandler<TRequest, TResponse> : PicoEndpointRequestHandler
{
    public PicoEndpointRequestHandler(Type picoEndpointType)
        : base(picoEndpointType)
    {
    }

    public Task ProcessAsync(HttpContext httpContext, TRequest request)
    {
        var endpoint = (PicoEndpoint<TRequest, TResponse>) httpContext.RequestServices.GetRequiredService(EndpointType);

        return endpoint.ProcessAsync(httpContext, request);
    }
}
