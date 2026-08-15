using AbyssRpg.Application.Characters.Repositories;
using AbyssRpg.Application.Common.Exceptions;
using AbyssRpg.Domain.Characters.Entities;
using AbyssRpg.Domain.Disciplines.Entities;

namespace AbyssRpg.Application.Characters.GetById;

public sealed class GetCharacterByIdHandler
{
	private readonly ICharacterRepository _characterRepository;

	public GetCharacterByIdHandler(
		ICharacterRepository characterRepository)
	{
		_characterRepository = characterRepository;
	}

	public async Task<GetCharacterByIdResult> HandleAsync(
		GetCharacterByIdQuery query,
		CancellationToken cancellationToken = default)
	{
		Character? character =
			await _characterRepository.GetByIdAsync(query.CharacterId, cancellationToken);

		if (character is null)
			throw new NotFoundException($"Personagem '{query.CharacterId}' não encontrado.");

		IReadOnlyCollection<CharacterDisciplineResult> disciplines =
			character.Disciplines
				.Select(MapDiscipline)
				.ToArray();

		return new GetCharacterByIdResult(
			character.Id,
			character.Name,
			character.Level,
			character.Experience,
			character.CurrentHealth,
			character.MaximumHealth,
			disciplines
		);
	}

	private static CharacterDisciplineResult MapDiscipline(
		Discipline discipline)
	{
		return new CharacterDisciplineResult(
			discipline.Type.ToString(),
			discipline.Level,
			discipline.Experience,
			discipline.GetExperienceRequiredForNextLevel()
		);
	}
}