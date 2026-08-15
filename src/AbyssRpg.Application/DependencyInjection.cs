using AbyssRpg.Application.Characters.Create;
using Microsoft.Extensions.DependencyInjection;

namespace AbyssRpg.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<CreateCharacterHandler>();

		return services;
	}
}