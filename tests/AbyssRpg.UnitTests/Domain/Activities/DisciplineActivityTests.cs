using AbyssRpg.Domain.Activities.Entities;
using AbyssRpg.Domain.Activities.Enums;
using AbyssRpg.Domain.Activities.Exceptions;
using AbyssRpg.Domain.Disciplines.Enums;

namespace AbyssRpg.UnitTests.Domain.Activities;

public sealed class DisciplineActivityTests
{
	[Fact]
	public void Start_ShouldCreateActivityWithInitialValues()
	{
		Guid characterId = Guid.NewGuid();

		DateTime startedAt = new(
			2026,
			8,
			4,
			10,
			0,
			0,
			DateTimeKind.Utc
		);

		DisciplineActivityDefinition definition =
			DisciplineActivityDefinition.Create(
				"occultism-study-grimoire",
				"Estudar grimório",
				DisciplineType.Occultism,
				TimeSpan.FromHours(8),
				6m
			);

		DisciplineActivity activity =
			DisciplineActivity.Start(
				characterId,
				definition,
				startedAt
			);
		

		Assert.NotEqual(Guid.Empty, activity.Id);
		Assert.Equal(characterId, activity.CharacterId);
		Assert.Equal("Estudar grimório", activity.Name);
		Assert.Equal(DisciplineType.Occultism, activity.DisciplineType);
		Assert.Equal(TimeSpan.FromHours(8), activity.Duration);
		Assert.Equal(56, activity.ExperienceReward);
		Assert.Equal(startedAt, activity.StartedAt);
		Assert.Equal(startedAt.AddHours(8), activity.EndsAt);
		Assert.Equal(ActivityStatus.InProgress, activity.Status);
		Assert.Null(activity.CompletedAt);
		Assert.Null(activity.CancelledAt);
	}

	[Fact]
	public void Complete_ShouldCompleteActivity_WhenRequiredTimeHasPassed()
	{
		DateTime startedAt = DateTime.UtcNow;

		DisciplineActivity activity = CreateActivity(startedAt);

		DateTime completedAt = startedAt.AddHours(8);

		activity.Complete(completedAt);

		Assert.Equal(ActivityStatus.Completed, activity.Status);
		Assert.Equal(completedAt, activity.CompletedAt);
	}

	[Fact]
	public void Complete_ShouldThrowException_WhenRequiredTimeHasNotPassed()
	{
		DateTime startedAt = DateTime.UtcNow;

		DisciplineActivity activity = CreateActivity(startedAt);

		ActivityException exception = Assert.Throws<ActivityException>(
			() => activity.Complete(
				startedAt.AddHours(7)
			)
		);

		Assert.Equal(
			"A atividade ainda não atingiu o tempo necessário para conclusão.",
			exception.Message
		);
	}

	[Fact]
	public void Complete_ShouldThrowException_WhenActivityWasAlreadyCompleted()
	{
		DateTime startedAt = DateTime.UtcNow;

		DisciplineActivity activity = CreateActivity(startedAt);

		activity.Complete(startedAt.AddHours(8));

		Assert.Throws<ActivityException>(
			() => activity.Complete(
				startedAt.AddHours(9)
			)
		);
	}

	[Fact]
	public void Cancel_ShouldCancelActivity()
	{
		DateTime startedAt = DateTime.UtcNow;

		DisciplineActivity activity = CreateActivity(startedAt);

		DateTime cancelledAt = startedAt.AddHours(2);

		activity.Cancel(cancelledAt);

		Assert.Equal(ActivityStatus.Cancelled, activity.Status);
		Assert.Equal(cancelledAt, activity.CancelledAt);
	}

	[Fact]
	public void Complete_ShouldThrowException_WhenActivityWasCancelled()
	{
		DateTime startedAt = DateTime.UtcNow;

		DisciplineActivity activity = CreateActivity(startedAt);

		activity.Cancel(startedAt.AddHours(2));

		Assert.Throws<ActivityException>(
			() => activity.Complete(
				startedAt.AddHours(8)
			)
		);
	}

	[Fact]
	public void GetRemainingTime_ShouldReturnRemainingDuration()
	{
		DateTime startedAt = DateTime.UtcNow;

		DisciplineActivity activity = CreateActivity(startedAt);

		TimeSpan remainingTime = activity.GetRemainingTime(
			startedAt.AddHours(3)
		);

		Assert.Equal(
			TimeSpan.FromHours(5),
			remainingTime
		);
	}

	[Fact]
	public void CanBeCompleted_ShouldReturnTrue_WhenRequiredTimeHasPassed()
	{
		DateTime startedAt = DateTime.UtcNow;

		DisciplineActivity activity = CreateActivity(startedAt);

		bool result = activity.CanBeCompleted(
			startedAt.AddHours(8)
		);

		Assert.True(result);
	}

	private static DisciplineActivity CreateActivity(
	DateTime startedAt)
	{
		DisciplineActivityDefinition definition =
			DisciplineActivityDefinition.Create(
				"occultism-study-grimoire",
				"Estudar grimório",
				DisciplineType.Occultism,
				TimeSpan.FromHours(8),
				6m
			);

		return DisciplineActivity.Start(
			Guid.NewGuid(),
			definition,
			startedAt
		);
	}
}