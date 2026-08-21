using AbyssRpg.Application.Characters.Repositories;
using AbyssRpg.Application.Common.Exceptions;
using AbyssRpg.Domain.Characters.Entities;

namespace AbyssRpg.Application.Characters.Create;

public sealed class CreateCharacterHandler
{
	private readonly ICharacterRepository _characterRepository;

	public CreateCharacterHandler( ICharacterRepository characterRepository)
	{
		_characterRepository = characterRepository;
	}

	public async Task<CreateCharacterResult> HandleAsync( CreateCharacterCommand command, CancellationToken cancellationToken = default)
	{
		Character character = Character.Create(command.Name);
		bool nameAlreadyExists = await _characterRepository.ExistsByNameAsync( command.Name, cancellationToken );
		if (nameAlreadyExists)
			throw new ConflictException( $"Já existe um personagem com o nome '{command.Name.Trim()}'." );

		await _characterRepository.AddAsync(
			character,
			cancellationToken
		);

		return new CreateCharacterResult(
			character.Id,
			character.Name.Value,
			character.Level,
			character.CurrentHealth,
			character.MaximumHealth
		);
	}
}