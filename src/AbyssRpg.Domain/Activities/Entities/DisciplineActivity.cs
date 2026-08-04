using AbyssRpg.Domain.Activities.Enums;
using AbyssRpg.Domain.Activities.Exceptions;
using AbyssRpg.Domain.Disciplines.Enums;

namespace AbyssRpg.Domain.Activities.Entities;

public sealed class DisciplineActivity
{
	private DisciplineActivity()
	{
	}

	private DisciplineActivity(
		Guid id,
		Guid characterId,
		string name,
		DisciplineType disciplineType,
		TimeSpan duration,
		int experienceReward,
		DateTime startedAt)
	{
		ValidateCharacterId(characterId);
		ValidateName(name);
		ValidateDuration(duration);
		ValidateExperienceReward(experienceReward);

		Id = id;
		CharacterId = characterId;
		Name = name.Trim();
		DisciplineType = disciplineType;
		Duration = duration;
		ExperienceReward = experienceReward;
		StartedAt = startedAt;
		EndsAt = startedAt.Add(duration);
		Status = ActivityStatus.InProgress;
	}

	public Guid Id { get; private set; }

	public Guid CharacterId { get; private set; }

	public string Name { get; private set; } = string.Empty;

	public DisciplineType DisciplineType { get; private set; }

	public TimeSpan Duration { get; private set; }

	public int ExperienceReward { get; private set; }

	public DateTime StartedAt { get; private set; }

	public DateTime EndsAt { get; private set; }

	public DateTime? CompletedAt { get; private set; }

	public DateTime? CancelledAt { get; private set; }

	public ActivityStatus Status { get; private set; }

	public static DisciplineActivity Start(
		Guid characterId,
		string name,
		DisciplineType disciplineType,
		TimeSpan duration,
		int experienceReward,
		DateTime startedAt)
	{
		return new DisciplineActivity(
			Guid.NewGuid(),
			characterId,
			name,
			disciplineType,
			duration,
			experienceReward,
			startedAt
		);
	}

	public void Complete(DateTime completedAt)
	{
		if (Status == ActivityStatus.Completed)
		{
			throw new ActivityException(
				"A atividade já foi concluída."
			);
		}

		if (Status == ActivityStatus.Cancelled)
		{
			throw new ActivityException(
				"Uma atividade cancelada não pode ser concluída."
			);
		}

		if (completedAt < EndsAt)
		{
			throw new ActivityException(
				"A atividade ainda não atingiu o tempo necessário para conclusão."
			);
		}

		Status = ActivityStatus.Completed;
		CompletedAt = completedAt;
	}

	public void Cancel(DateTime cancelledAt)
	{
		if (Status == ActivityStatus.Completed)
		{
			throw new ActivityException(
				"Uma atividade concluída não pode ser cancelada."
			);
		}

		if (Status == ActivityStatus.Cancelled)
		{
			throw new ActivityException(
				"A atividade já foi cancelada."
			);
		}

		if (cancelledAt < StartedAt)
		{
			throw new ActivityException(
				"A data de cancelamento não pode ser anterior ao início da atividade."
			);
		}

		Status = ActivityStatus.Cancelled;
		CancelledAt = cancelledAt;
	}

	public bool CanBeCompleted(DateTime currentDate)
	{
		return Status == ActivityStatus.InProgress
			&& currentDate >= EndsAt;
	}

	public TimeSpan GetRemainingTime(DateTime currentDate)
	{
		if (Status != ActivityStatus.InProgress)
		{
			return TimeSpan.Zero;
		}

		if (currentDate >= EndsAt)
		{
			return TimeSpan.Zero;
		}

		return EndsAt - currentDate;
	}

	private static void ValidateCharacterId(Guid characterId)
	{
		if (characterId == Guid.Empty)
		{
			throw new ActivityException(
				"O identificador do personagem é obrigatório."
			);
		}
	}

	private static void ValidateName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ActivityException(
				"O nome da atividade é obrigatório."
			);
		}

		if (name.Trim().Length > 100)
		{
			throw new ActivityException(
				"O nome da atividade deve possuir no máximo 100 caracteres."
			);
		}
	}

	private static void ValidateDuration(TimeSpan duration)
	{
		if (duration <= TimeSpan.Zero)
		{
			throw new ActivityException(
				"A duração da atividade deve ser maior que zero."
			);
		}
	}

	private static void ValidateExperienceReward(int experienceReward)
	{
		if (experienceReward <= 0)
		{
			throw new ActivityException(
				"A recompensa de experiência deve ser maior que zero."
			);
		}
	}
}