using AbyssRpg.Application.Characters.Repositories;
using AbyssRpg.Infrastructure.Characters.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AbyssRpg.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services)
	{
		services.AddSingleton<ICharacterRepository, InMemoryCharacterRepository>();

		return services;
	}
}