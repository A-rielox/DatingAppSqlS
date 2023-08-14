using API.Interfaces;
using API.Services;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddAplicationServices(
                            this IServiceCollection services,
                            IConfiguration config
                            )
    {
        services.AddCors();

        services.AddScoped<ITokenService, TokenService>();


        //services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

        //services.AddScoped<IPhotoService, PhotoService>();
        //services.AddScoped<ILikesRepository, LikesRepository>();
        //services.AddScoped<IMessageRepository, MessageRepository>();
        //services.AddScoped<LogUserActivity>();
        //services.AddScoped<IUserRepository, UserRepository>();

        //services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

        return services;
    }
}
