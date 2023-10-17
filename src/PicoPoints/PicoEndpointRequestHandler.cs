using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace PicoPoints;

internal abstract class PicoEndpointRequestHandler
{
    protected readonly Type EndpointType;

    protected PicoEndpointRequestHandler(Type endpointType)
    {
        EndpointType = endpointType;
    }

    internal abstract Delegate CreateDelegate();

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

    internal override Delegate CreateDelegate()
    {
        return (HttpContext context, [FromBody] TRequest request) => ProcessAsync(context, request);
    }

    private Task ProcessAsync(HttpContext httpContext, TRequest request)
    {
        var endpoint = (PicoEndpoint<TRequest, TResponse>) httpContext.RequestServices.GetRequiredService(EndpointType);

        return endpoint.ProcessAsync(httpContext, request);
    }
}
