using System.Linq.Expressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace PicoPoints;

public static class WebApplicationExtensions
{
    public static WebApplication UsePicoPoints(this WebApplication app)
    {
        var endpointRequestHandlers = (IEnumerable<PicoEndpointRequestHandler>) app.Services.GetServices(typeof(PicoEndpointRequestHandler));

        foreach (var endpointRequestHandler in endpointRequestHandlers)
        {
        }

        return app;
    }
}