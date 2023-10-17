using Microsoft.AspNetCore.Http;

namespace PicoPoints;

public abstract class PicoEndpoint
{
    public abstract PicoEndpointConfiguration Configure();
}

public abstract class PicoEndpoint<TRequest, TResponse> : PicoEndpoint
{
    internal async Task ProcessAsync(HttpContext httpContext, TRequest request, CancellationToken cancellationToken = default)
    {
        var result = await HandleAsync(request);
        await result.ExecuteAsync(httpContext);
    }

    protected abstract Task<PicoEndpointResult<TResponse>> HandleAsync(TRequest request);

    protected PicoEndpointResult<TResponse> Ok(TResponse response)
    {
        return new PicoOkEndpointResult<TResponse>(response);
    }
}