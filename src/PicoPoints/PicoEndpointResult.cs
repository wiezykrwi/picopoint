using Microsoft.AspNetCore.Http;

namespace PicoPoints;

public abstract class PicoEndpointResult<TResponse>
{
    public abstract Task ExecuteAsync(HttpContext httpContext);
}

public sealed class PicoOkEndpointResult<TResponse> : PicoEndpointResult<TResponse>
{
    private readonly TResponse _response;

    public PicoOkEndpointResult(TResponse response)
    {
        _response = response;
    }

    public override async Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = 200;
        await httpContext.Response.WriteAsJsonAsync(_response);
    }
}