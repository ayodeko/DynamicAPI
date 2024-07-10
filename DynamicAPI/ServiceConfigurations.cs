namespace DynamicAPI;

public static class ServiceConfigurations
{
    
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<ILogicFactory, LogicFactory>();
        return services;
    }
}