using System.Linq.Expressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
            var buildExpression = endpointRequestHandler.BuildExpression();
            app.MapMethods(configuration.Route, new[] { configuration.Method }, buildExpression);
        }

        return app;
    }
}