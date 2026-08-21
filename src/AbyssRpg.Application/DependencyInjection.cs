using AbyssRpg.Application.Characters.Create;
using AbyssRpg.Application.Characters.GetById;
using Microsoft.Extensions.DependencyInjection;

namespace AbyssRpg.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<CreateCharacterHandler>();
		services.AddScoped<GetCharacterByIdHandler>();

		return services;
	}
}