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
            var configuration = endpointRequestHandler.GetConfiguration(app.Services);
            app.MapMethods(configuration.Route, new[] { configuration.Method }, endpointRequestHandler.CreateDelegate());
        }

        return app;
    }
}