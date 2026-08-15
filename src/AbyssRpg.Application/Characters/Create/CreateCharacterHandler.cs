using AbyssRpg.Application.Characters.Repositories;
using AbyssRpg.Domain.Characters.Entities;

namespace AbyssRpg.Application.Characters.Create;

public sealed class CreateCharacterHandler
{
	private readonly ICharacterRepository _characterRepository;

	public CreateCharacterHandler(
		ICharacterRepository characterRepository)
	{
		_characterRepository = characterRepository;
	}

	public async Task<CreateCharacterResult> HandleAsync(
		CreateCharacterCommand command,
		CancellationToken cancellationToken = default)
	{
		Character character = Character.Create(command.Name);

		await _characterRepository.AddAsync(
			character,
			cancellationToken
		);

		return new CreateCharacterResult(
			character.Id,
			character.Name,
			character.Level,
			character.CurrentHealth,
			character.MaximumHealth
		);
	}
}