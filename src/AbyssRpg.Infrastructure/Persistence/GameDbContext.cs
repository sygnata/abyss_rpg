using AbyssRpg.Domain.Characters.Entities;
using AbyssRpg.Domain.Disciplines.Entities;
using Microsoft.EntityFrameworkCore;

namespace AbyssRpg.Infrastructure.Persistence;

public sealed class GameDbContext : DbContext
{
	public GameDbContext( DbContextOptions<GameDbContext> options) : base(options)
	{
	}

	public DbSet<Character> Characters =>
		Set<Character>();

	public DbSet<Discipline> Disciplines =>
		Set<Discipline>();

	protected override void OnModelCreating( ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly( typeof(GameDbContext).Assembly );

		base.OnModelCreating(modelBuilder);
	}
}