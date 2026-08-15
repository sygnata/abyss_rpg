using AbyssRpg.Domain.Characters.Entities;

namespace AbyssRpg.Application.Characters.Repositories;

public interface ICharacterRepository
{
	Task AddAsync(Character character, CancellationToken cancellationToken = default);

	Task<Character?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

	Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);

}