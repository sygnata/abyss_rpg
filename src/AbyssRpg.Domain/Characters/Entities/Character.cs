using AbyssRpg.Domain.Characters.Exceptions;
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
		Name = ValidateName(name);

		Level = InitialLevel;
		Experience = InitialExperience;

		MaximumHealth = CalculateMaximumHealth(Level);
		CurrentHealth = MaximumHealth;

		CreatedAt = DateTime.UtcNow;

		InitializeDisciplines();
	}

	public Guid Id { get; private set; }

	public string Name { get; private set; } = string.Empty;

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
		{
			throw new CharacterException(
				"A quantidade de experiência deve ser maior que zero."
			);
		}

		Experience += amount;

		ProcessLevelUps();
	}

	public void GainDisciplineExperience(
		DisciplineType disciplineType,
		int amount)
	{
		Discipline discipline = GetDiscipline(disciplineType);

		discipline.GainExperience(amount);
	}

	public Discipline GetDiscipline(DisciplineType disciplineType)
	{
		Discipline? discipline = _disciplines.FirstOrDefault(
			currentDiscipline =>
				currentDiscipline.Type == disciplineType
		);

		if (discipline is null)
		{
			throw new CharacterException(
				$"A disciplina {disciplineType} não pertence ao personagem."
			);
		}

		return discipline;
	}

	public void ReceiveDamage(int damage)
	{
		if (damage <= 0)
		{
			throw new CharacterException(
				"O dano recebido deve ser maior que zero."
			);
		}

		CurrentHealth = Math.Max(0, CurrentHealth - damage);
	}

	public void RestoreHealth(int amount)
	{
		if (amount <= 0)
		{
			throw new CharacterException(
				"A quantidade de vida recuperada deve ser maior que zero."
			);
		}

		CurrentHealth = Math.Min(
			MaximumHealth,
			CurrentHealth + amount
		);
	}

	public bool IsAlive()
	{
		return CurrentHealth > 0;
	}

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
		{
			throw new CharacterException(
				$"O personagem já possui a disciplina {disciplineType}."
			);
		}

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

	private static int CalculateMaximumHealth(int level)
	{
		return BaseMaximumHealth
			+ ((level - 1) * HealthIncreasePerLevel);
	}

	private static int CalculateExperienceRequired(int level)
	{
		return 100 + ((level - 1) * 50);
	}

	private static string ValidateName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new CharacterException(
				"O nome do personagem é obrigatório."
			);
		}

		string normalizedName = name.Trim();

		if (normalizedName.Length < 3)
		{
			throw new CharacterException(
				"O nome do personagem deve possuir pelo menos 3 caracteres."
			);
		}

		if (normalizedName.Length > 30)
		{
			throw new CharacterException(
				"O nome do personagem deve possuir no máximo 30 caracteres."
			);
		}

		return normalizedName;
	}
}