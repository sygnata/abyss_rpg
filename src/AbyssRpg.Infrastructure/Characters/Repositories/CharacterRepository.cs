using AbyssRpg.Application.Characters.Repositories;
using AbyssRpg.Domain.Characters.Entities;
using AbyssRpg.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbyssRpg.Infrastructure.Characters.Repositories;

public sealed class CharacterRepository : ICharacterRepository
{
	private readonly GameDbContext _dbContext;

	public CharacterRepository( GameDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task AddAsync( Character character, CancellationToken cancellationToken = default)
	{
		await _dbContext.Characters.AddAsync( character, cancellationToken );

		await _dbContext.SaveChangesAsync( cancellationToken );
	}

	public async Task<Character?> GetByIdAsync( Guid id, CancellationToken cancellationToken = default)
	{
		return await _dbContext.Characters
			.Include(character => character.Disciplines)
			.FirstOrDefaultAsync(
				character => character.Id == id,
				cancellationToken
			);
	}

	public async Task<bool> ExistsByNameAsync( string name, CancellationToken cancellationToken = default)
	{
		string normalizedName = name.Trim().ToLowerInvariant();

		return await _dbContext.Database
			.SqlQuery<int>(
				$"""
            SELECT 1 AS "Value"
            FROM characters
            WHERE LOWER(name) = {normalizedName}
            LIMIT 1
            """
			)
			.AnyAsync(cancellationToken);
	}
}