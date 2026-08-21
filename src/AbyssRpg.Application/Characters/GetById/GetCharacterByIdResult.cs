namespace AbyssRpg.Application.Characters.GetById;

public sealed record GetCharacterByIdResult(
	Guid Id,
	string Name,
	int Level,
	int Experience,
	int CurrentHealth,
	int MaximumHealth,
	IReadOnlyCollection<CharacterDisciplineResult> Disciplines
);

public sealed record CharacterDisciplineResult(
	string Type,
	int Level,
	int Experience,
	int ExperienceRequiredForNextLevel
);