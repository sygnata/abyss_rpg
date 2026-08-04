using AbyssRpg.Domain.Disciplines.Exceptions;
using AbyssRpg.Domain.Disciplines.Services;

namespace AbyssRpg.UnitTests.Domain.Disciplines;

public sealed class DisciplineExperienceCalculatorTests
{
	[Theory]
	[InlineData(1, 20)]
	[InlineData(2, 30)]
	[InlineData(3, 42)]
	[InlineData(4, 58)]
	[InlineData(5, 76)]
	[InlineData(10, 214)]
	public void CalculateRequiredExperience_ShouldReturnExpectedValue(int level, int expectedExperience)
	{
		int result =
			DisciplineExperienceCalculator.CalculateRequiredExperience(level);

		Assert.Equal(expectedExperience, result);
	}

	[Theory]
	[InlineData(0)]
	[InlineData(-1)]
	public void CalculateRequiredExperience_ShouldThrowException_WhenLevelIsInvalid(
		int level)
	{
		Assert.Throws<DisciplineException>(
			() => DisciplineExperienceCalculator
				.CalculateRequiredExperience(level)
		);
	}
}