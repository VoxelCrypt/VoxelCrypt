using Kernel.Services;
using Microsoft.Extensions.DependencyInjection;
using Service.Services;

namespace Service.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddVoxelCryptGatewayService(this IServiceCollection services)
    {
        services.AddSingleton<IGatewayPortService, GatewayAdapterService>();

        return services;
    }
}
