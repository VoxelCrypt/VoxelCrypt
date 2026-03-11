using Kernel.Services;
using Microsoft.Extensions.DependencyInjection;
using Service.Services;
using Wolverine;

namespace Service.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddVoxelCryptSampleService(this IServiceCollection services)
    {
        services.AddSingleton<ISamplePortService, SampleAdapterService>();

        services.AddWolverine(_ =>
        {
        });

        return services;
    }
}
