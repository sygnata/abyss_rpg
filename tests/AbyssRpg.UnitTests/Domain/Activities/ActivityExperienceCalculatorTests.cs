using AbyssRpg.Domain.Activities.Exceptions;
using AbyssRpg.Domain.Activities.Services;

namespace AbyssRpg.UnitTests.Domain.Activities;

public sealed class ActivityExperienceCalculatorTests
{
	[Theory]
	[InlineData(0.5, 3)]
	[InlineData(1, 6)]
	[InlineData(2, 13)]
	[InlineData(4, 27)]
	[InlineData(8, 56)]
	[InlineData(12, 87)]
	public void CalculateReward_ShouldApplyDurationBonus(
		double durationInHours,
		int expectedExperience)
	{
		int result =
			ActivityExperienceCalculator.CalculateReward(
				TimeSpan.FromHours(durationInHours),
				6m
			);

		Assert.Equal(expectedExperience, result);
	}

	[Theory]
	[InlineData(0.5, 1.00)]
	[InlineData(1, 1.00)]
	[InlineData(2, 1.05)]
	[InlineData(4, 1.10)]
	[InlineData(8, 1.15)]
	[InlineData(12, 1.20)]
	public void GetDurationMultiplier_ShouldReturnExpectedMultiplier(
		double durationInHours,
		double expectedMultiplier)
	{
		decimal result =
			ActivityExperienceCalculator.GetDurationMultiplier(
				TimeSpan.FromHours(durationInHours)
			);

		Assert.Equal(
			(decimal)expectedMultiplier,
			result
		);
	}

	[Fact]
	public void CalculateReward_ShouldThrowException_WhenDurationIsInvalid()
	{
		Assert.Throws<ActivityException>(
			() => ActivityExperienceCalculator.CalculateReward(
				TimeSpan.Zero,
				6m
			)
		);
	}

	[Fact]
	public void CalculateReward_ShouldThrowException_WhenExperiencePerHourIsInvalid()
	{
		Assert.Throws<ActivityException>(
			() => ActivityExperienceCalculator.CalculateReward(
				TimeSpan.FromHours(1),
				0
			)
		);
	}
}