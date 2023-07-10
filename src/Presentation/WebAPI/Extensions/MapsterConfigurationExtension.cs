using HomeInv.Persistence.Dbo;
using Mapster;
using WebAPI.Dtos;

namespace WebAPI.Extensions;

public static class MapsterConfigurationExtension
{
    public static void RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<User, UserRegistrationDto>
            .ForType();
    }
}
