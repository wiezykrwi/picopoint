using Microsoft.AspNetCore.Http;

namespace PicoPoints;

public abstract class PicoEndpoint
{
    public abstract PicoEndpointConfiguration Configure();
}

public abstract class PicoEndpoint<TRequest, TResult> : PicoEndpoint
{
    internal Task ProcessAsync(HttpContext httpContext, TRequest request, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
