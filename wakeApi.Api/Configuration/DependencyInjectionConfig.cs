
using Microsoft.EntityFrameworkCore;
using wakeDomain.Domain.Interfaces.Repository;
using wakeDomain.Domain.Interfaces.Service;
using wakeDomain.Domain.Service;
using wakeInfra.Infra.Context;
using wakeInfra.Infra.Repository;

public static class DependencyInjectionConfig
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
                                                                 IConfiguration configuration)
    {

        services.AddControllers();

        //DB
        services.AddDbContext<ProductContext>(options =>
                        options.UseInMemoryDatabase("ProductsDB"));

        //Product
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}