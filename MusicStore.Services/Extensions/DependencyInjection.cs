using Microsoft.Extensions.DependencyInjection;
using MusicStore.Repositories;
using MusicStore.Services.Implementations;
using MusicStore.Services.Interfaces;

namespace MusicStore.Services.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddTransient<IGenreRepository, GenreRepository>();
        services.AddTransient<IConcertRepository, ConcertRepository>();
        services.AddTransient<ISaleRepository, SaleRepository>();
        services.AddTransient<ICustomerRepository, CustomerRepository>();
        
        services.AddTransient<IGenreService, GenreService>();
        services.AddTransient<IConcertService, ConcertService>();
        services.AddTransient<ISaleService, SaleService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IEmailSender, EmailSender>();

        return services;
    }

    public static IServiceCollection AddFileUploader(this IServiceCollection services, bool isDevelopment)
    {
        if (isDevelopment)
            services.AddTransient<IFileUploader, FileUploader>();
        else
            services.AddTransient<IFileUploader, AzureBlobStorageUploader>();

        return services;
    }
}