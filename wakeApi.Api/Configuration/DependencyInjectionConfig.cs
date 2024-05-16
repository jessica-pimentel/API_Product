
public static class DependencyInjectionConfig
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
                                                                 IConfiguration configuration)
    {
        //Product
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}