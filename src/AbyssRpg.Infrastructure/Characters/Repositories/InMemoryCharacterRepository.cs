using System.Collections.Concurrent;
using AbyssRpg.Application.Characters.Repositories;
using AbyssRpg.Domain.Characters.Entities;
using AbyssRpg.Application;
using AbyssRpg.Infrastructure;

namespace AbyssRpg.Infrastructure.Characters.Repositories;

public sealed class InMemoryCharacterRepository	: ICharacterRepository
{
	private readonly ConcurrentDictionary<Guid, Character>
		_characters = new();

	public Task AddAsync(
		Character character,
		CancellationToken cancellationToken = default)
	{
		_characters[character.Id] = character;

		return Task.CompletedTask;
	}

	public Task<Character?> GetByIdAsync(
		Guid id,
		CancellationToken cancellationToken = default)
	{
		_characters.TryGetValue(
			id,
			out Character? character
		);

		return Task.FromResult(character);
	}
}