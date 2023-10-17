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
            while (!picoEndpointType.IsGenericType || picoEndpointType.GetGenericTypeDefinition() != typeof(PicoEndpoint<,>))
            {
                picoEndpointType = picoEndpointType.BaseType ?? throw new ArgumentException("Invalid Endpoint definition");
            }

            var type = typeof(PicoEndpointRequestHandler<,>);
            var closedType = type.MakeGenericType(picoEndpointType.GenericTypeArguments);
            var instance = Activator.CreateInstance(closedType, endpointType) ?? throw new Exception("Internal error - could not create instance of request handler");

            services.AddSingleton(typeof(PicoEndpointRequestHandler), instance);
        }

        return services;
    }
}