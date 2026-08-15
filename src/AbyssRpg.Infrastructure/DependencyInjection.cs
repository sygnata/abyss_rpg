using AbyssRpg.Application.Characters.Repositories;
using AbyssRpg.Infrastructure.Characters.Repositories;
using AbyssRpg.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AbyssRpg.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure( this IServiceCollection services, IConfiguration configuration)
	{
		string? connectionString = 
			configuration
				.GetConnectionString( "PostgreSql" );

		if (string.IsNullOrWhiteSpace(connectionString))
			throw new InvalidOperationException( "A connection string 'PostgreSql' não foi configurada." );

		services.AddDbContext<GameDbContext>(
			options =>
				options.UseNpgsql(connectionString)
		);

		services.AddScoped< ICharacterRepository, CharacterRepository>();

		return services;
	}
}