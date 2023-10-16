using Microsoft.Extensions.DependencyInjection;

namespace PicoPoints;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPicoPoints(this IServiceCollection services, Action<PicoPointsConfiguration> configureAction)
    {
        var configuration = new PicoPointsConfiguration();
        configureAction?.Invoke(configuration);

        foreach (var endpointType in configuration.Endpoints)
        {
            services.AddTransient(endpointType);
            
            var picoEndpointType = endpointType;
            while (picoEndpointType != typeof(PicoEndpoint<,>))
            {
                if (picoEndpointType.BaseType == null) throw new ArgumentException("Invalid Endpoint definition");
                picoEndpointType = picoEndpointType.BaseType;
            }

            var type = typeof(PicoEndpointRequestHandler<,>);
            var closedType = type.MakeGenericType(picoEndpointType.GenericTypeArguments);

            services.AddSingleton(typeof(PicoEndpointRequestHandler), closedType);
        }

        return services;
    }
}