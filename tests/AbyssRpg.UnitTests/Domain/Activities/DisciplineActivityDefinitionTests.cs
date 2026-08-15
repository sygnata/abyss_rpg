using AbyssRpg.Domain.Activities.Entities;
using AbyssRpg.Domain.Activities.Exceptions;
using AbyssRpg.Domain.Disciplines.Enums;

namespace AbyssRpg.UnitTests.Domain.Activities;

public sealed class DisciplineActivityDefinitionTests
{
	[Fact]
	public void Create_ShouldCreateDefinitionAndCalculateReward()
	{
		DisciplineActivityDefinition definition =
			DisciplineActivityDefinition.Create(
				"occultism-study-grimoire",
				"Estudar grimório",
				DisciplineType.Occultism,
				TimeSpan.FromHours(8),
				6m,
				5
			);

		Assert.NotEqual(Guid.Empty, definition.Id);
		Assert.Equal(
			"occultism-study-grimoire",
			definition.Code
		);
		Assert.Equal(
			"Estudar grimório",
			definition.Name
		);
		Assert.Equal(
			DisciplineType.Occultism,
			definition.DisciplineType
		);
		Assert.Equal(
			TimeSpan.FromHours(8),
			definition.Duration
		);
		Assert.Equal(6m, definition.ExperiencePerHour);
		Assert.Equal(56, definition.ExperienceReward);
		Assert.Equal(5, definition.MinimumDisciplineLevel);
	}

	[Fact]
	public void Create_ShouldNormalizeCode()
	{
		DisciplineActivityDefinition definition =
			DisciplineActivityDefinition.Create(
				"  OCCULTISM-STUDY-GRIMOIRE  ",
				"Estudar grimório",
				DisciplineType.Occultism,
				TimeSpan.FromHours(8),
				6m
			);

		Assert.Equal(
			"occultism-study-grimoire",
			definition.Code
		);
	}

	[Fact]
	public void Create_ShouldThrowException_WhenCodeIsEmpty()
	{
		Assert.Throws<ActivityException>(
			() => DisciplineActivityDefinition.Create(
				string.Empty,
				"Estudar grimório",
				DisciplineType.Occultism,
				TimeSpan.FromHours(8),
				6m
			)
		);
	}

	[Fact]
	public void Create_ShouldThrowException_WhenMinimumLevelIsInvalid()
	{
		Assert.Throws<ActivityException>(
			() => DisciplineActivityDefinition.Create(
				"occultism-study-grimoire",
				"Estudar grimório",
				DisciplineType.Occultism,
				TimeSpan.FromHours(8),
				6m,
				0
			)
		);
	}
}