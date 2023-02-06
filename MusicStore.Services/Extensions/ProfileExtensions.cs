using Microsoft.Extensions.DependencyInjection;
using MusicStore.Services.Profiles;

namespace MusicStore.Services.Extensions;

public static class ProfileExtensions
{
    public static void AddAutoMapperConfigurations(this IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            config.AddProfile<GenreProfile>();
            config.AddProfile<ConcertProfile>();
            config.AddProfile<SaleProfile>();
        });
    }
}