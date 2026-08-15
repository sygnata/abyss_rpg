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
}