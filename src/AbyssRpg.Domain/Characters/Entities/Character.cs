using AbyssRpg.Domain.Activities.Entities;
using AbyssRpg.Domain.Characters.Exceptions;
using AbyssRpg.Domain.Characters.ValueObjects;
using AbyssRpg.Domain.Disciplines.Entities;
using AbyssRpg.Domain.Disciplines.Enums;

namespace AbyssRpg.Domain.Characters.Entities;

public sealed class Character
{
	private const int InitialLevel = 1;
	private const int InitialExperience = 0;

	private const int BaseMaximumHealth = 100;
	private const int HealthIncreasePerLevel = 10;

	private readonly List<Discipline> _disciplines = [];

	private Character()
	{
	}

	private Character(Guid id, string name)
	{
		Id = id;
		Name = CharacterName.Create(name);

		Level = InitialLevel;
		Experience = InitialExperience;

		MaximumHealth = CalculateMaximumHealth(Level);
		CurrentHealth = MaximumHealth;

		CreatedAt = DateTime.UtcNow;

		InitializeDisciplines();
	}

	public Guid Id { get; private set; }

	public CharacterName Name { get; private set; } = null!;

	public int Level { get; private set; }

	public int Experience { get; private set; }

	public int CurrentHealth { get; private set; }

	public int MaximumHealth { get; private set; }

	public DateTime CreatedAt { get; private set; }

	public IReadOnlyCollection<Discipline> Disciplines =>
		_disciplines.AsReadOnly();

	public static Character Create(string name)
	{
		return new Character(
			Guid.NewGuid(),
			name
		);
	}

	public void GainExperience(int amount)
	{
		if (amount <= 0)
			throw new CharacterException( "A quantidade de experiência deve ser maior que zero." );

		Experience += amount;

		ProcessLevelUps();
	}

	public void CompleteDisciplineActivity(
		DisciplineActivity activity,
		DateTime completedAt)
	{
		ArgumentNullException.ThrowIfNull(activity);

		if (activity.CharacterId != Id)
			throw new CharacterException( "A atividade não pertence a este personagem." );

		activity.Complete(completedAt);

		Discipline discipline = GetDiscipline(
			activity.DisciplineType
		);

		discipline.GainExperience(
			activity.ExperienceReward
		);
	}

	public Discipline GetDiscipline(DisciplineType disciplineType)
	{
		Discipline? discipline = _disciplines.FirstOrDefault(
			currentDiscipline =>
				currentDiscipline.Type == disciplineType
		);

		if (discipline is null)
			throw new CharacterException( $"A disciplina {disciplineType} não pertence ao personagem." );

		return discipline;
	}

	public void ReceiveDamage(int damage)
	{
		if (damage <= 0)
			throw new CharacterException( "O dano recebido deve ser maior que zero." );

		CurrentHealth = Math.Max(0, CurrentHealth - damage);
	}

	public void RestoreHealth(int amount)
	{
		if (amount <= 0)
			throw new CharacterException( "A quantidade de vida recuperada deve ser maior que zero." );

		CurrentHealth = Math.Min(
			MaximumHealth,
			CurrentHealth + amount
		);
	}

	public bool IsAlive() =>
		CurrentHealth > 0;

	public int GetExperienceRequiredForNextLevel()
	{
		return CalculateExperienceRequired(Level);
	}

	private void InitializeDisciplines()
	{
		foreach (DisciplineType disciplineType in Enum.GetValues<DisciplineType>())
		{
			AddDiscipline(disciplineType);
		}
	}

	private void AddDiscipline(DisciplineType disciplineType)
	{
		bool disciplineAlreadyExists = _disciplines.Any(
			currentDiscipline =>
				currentDiscipline.Type == disciplineType
		);

		if (disciplineAlreadyExists)
			throw new CharacterException( $"O personagem já possui a disciplina {disciplineType}." );

		Discipline discipline = Discipline.Create(
			Id,
			disciplineType
		);

		_disciplines.Add(discipline);
	}

	private void ProcessLevelUps()
	{
		int requiredExperience = CalculateExperienceRequired(Level);

		while (Experience >= requiredExperience)
		{
			Experience -= requiredExperience;
			Level++;

			IncreaseMaximumHealth();

			requiredExperience = CalculateExperienceRequired(Level);
		}
	}

	private void IncreaseMaximumHealth()
	{
		int previousMaximumHealth = MaximumHealth;

		MaximumHealth = CalculateMaximumHealth(Level);

		int gainedHealth = MaximumHealth - previousMaximumHealth;

		CurrentHealth = Math.Min(
			MaximumHealth,
			CurrentHealth + gainedHealth
		);
	}

	private static int CalculateMaximumHealth(int level) =>
		BaseMaximumHealth + ((level - 1) * HealthIncreasePerLevel);

	private static int CalculateExperienceRequired(int level) =>
		100 + ((level - 1) * 50);


	public DisciplineActivity StartDisciplineActivity(DisciplineActivityDefinition definition, DateTime startedAt)
	{
		ArgumentNullException.ThrowIfNull(definition);

		Discipline discipline = GetDiscipline(definition.DisciplineType);

		if (discipline.Level < definition.MinimumDisciplineLevel)
			throw new CharacterException(
				$"A disciplina {definition.DisciplineType} precisa estar no nível " +
				$"{definition.MinimumDisciplineLevel} para iniciar esta atividade."
			);

		return DisciplineActivity.Start(
			Id,
			definition,
			startedAt
		);
	}

}