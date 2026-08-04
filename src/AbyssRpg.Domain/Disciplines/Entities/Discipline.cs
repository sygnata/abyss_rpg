using AbyssRpg.Domain.Disciplines.Enums;
using AbyssRpg.Domain.Disciplines.Exceptions;
using AbyssRpg.Domain.Disciplines.Services;

namespace AbyssRpg.Domain.Disciplines.Entities;

public sealed class Discipline
{
	private const int InitialLevel = 1;
	private const int InitialExperience = 0;

	private Discipline()
	{
	}

	private Discipline(
		Guid id,
		Guid characterId,
		DisciplineType type)
	{
		if (characterId == Guid.Empty)
		{
			throw new DisciplineException(
				"O identificador do personagem é obrigatório."
			);
		}

		Id = id;
		CharacterId = characterId;
		Type = type;
		Level = InitialLevel;
		Experience = InitialExperience;
		CreatedAt = DateTime.UtcNow;
	}

	public Guid Id { get; private set; }

	public Guid CharacterId { get; private set; }

	public DisciplineType Type { get; private set; }

	public int Level { get; private set; }

	public int Experience { get; private set; }

	public DateTime CreatedAt { get; private set; }

	public static Discipline Create(
		Guid characterId,
		DisciplineType type)
	{
		return new Discipline(
			Guid.NewGuid(),
			characterId,
			type
		);
	}

	public void GainExperience(int amount)
	{
		if (amount <= 0)
		{
			throw new DisciplineException(
				"A quantidade de experiência deve ser maior que zero."
			);
		}

		Experience += amount;

		ProcessLevelUps();
	}

	public int GetExperienceRequiredForNextLevel()
	{
		return DisciplineExperienceCalculator
			.CalculateRequiredExperience(Level);
	}

	public int GetRemainingExperienceForNextLevel()
	{
		int requiredExperience = GetExperienceRequiredForNextLevel();

		return Math.Max(
			0,
			requiredExperience - Experience
		);
	}

	private void ProcessLevelUps()
	{
		int requiredExperience = GetExperienceRequiredForNextLevel();

		while (Experience >= requiredExperience)
		{
			Experience -= requiredExperience;
			Level++;

			requiredExperience = GetExperienceRequiredForNextLevel();
		}
	}
}