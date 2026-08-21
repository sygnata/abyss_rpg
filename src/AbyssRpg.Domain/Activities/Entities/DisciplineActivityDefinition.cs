using AbyssRpg.Domain.Activities.Exceptions;
using AbyssRpg.Domain.Activities.Services;
using AbyssRpg.Domain.Disciplines.Enums;

namespace AbyssRpg.Domain.Activities.Entities;

public sealed class DisciplineActivityDefinition
{
	private DisciplineActivityDefinition()
	{
	}

	private DisciplineActivityDefinition(
		Guid id,
		string code,
		string name,
		DisciplineType disciplineType,
		TimeSpan duration,
		decimal experiencePerHour,
		int minimumDisciplineLevel)
	{
		Id = id;
		Code = ValidateCode(code);
		Name = ValidateName(name);
		DisciplineType = disciplineType;
		Duration = ValidateDuration(duration);
		ExperiencePerHour =
			ValidateExperiencePerHour(experiencePerHour);

		MinimumDisciplineLevel =
			ValidateMinimumDisciplineLevel(
				minimumDisciplineLevel
			);

		ExperienceReward =
			ActivityExperienceCalculator.CalculateReward(
				Duration,
				ExperiencePerHour
			);
	}

	public Guid Id { get; private set; }

	public string Code { get; private set; } = string.Empty;

	public string Name { get; private set; } = string.Empty;

	public DisciplineType DisciplineType { get; private set; }

	public TimeSpan Duration { get; private set; }

	public decimal ExperiencePerHour { get; private set; }

	public int ExperienceReward { get; private set; }

	public int MinimumDisciplineLevel { get; private set; }

	public static DisciplineActivityDefinition Create(
		string code,
		string name,
		DisciplineType disciplineType,
		TimeSpan duration,
		decimal experiencePerHour,
		int minimumDisciplineLevel = 1)
	{
		return new DisciplineActivityDefinition(
			Guid.NewGuid(),
			code,
			name,
			disciplineType,
			duration,
			experiencePerHour,
			minimumDisciplineLevel
		);
	}

	private static string ValidateCode(string code)
	{
		if (string.IsNullOrWhiteSpace(code))
			throw new ActivityException("O código da atividade é obrigatório.");

		string normalizedCode = code.Trim().ToLowerInvariant();

		if (normalizedCode.Length > 50)
			throw new ActivityException("O código da atividade deve possuir no máximo 50 caracteres.");

		return normalizedCode;
	}

	private static string ValidateName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ActivityException("O nome da atividade é obrigatório.");

		string normalizedName = name.Trim();

		if (normalizedName.Length > 100)
			throw new ActivityException("O nome da atividade deve possuir no máximo 100 caracteres.");

		return normalizedName;
	}

	private static TimeSpan ValidateDuration(TimeSpan duration)
	{
		if (duration <= TimeSpan.Zero)
			throw new ActivityException("A duração da atividade deve ser maior que zero.");

		return duration;
	}

	private static decimal ValidateExperiencePerHour(decimal experiencePerHour)
	{
		if (experiencePerHour <= 0)
			throw new ActivityException("A experiência por hora deve ser maior que zero.");

		return experiencePerHour;
	}

	private static int ValidateMinimumDisciplineLevel(int minimumDisciplineLevel)
	{
		if (minimumDisciplineLevel <= 0)
			throw new ActivityException("O nível mínimo da disciplina deve ser maior que zero.");

		return minimumDisciplineLevel;
	}
}