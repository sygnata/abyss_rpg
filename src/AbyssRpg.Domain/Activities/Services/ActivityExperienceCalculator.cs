using AbyssRpg.Domain.Activities.Exceptions;

namespace AbyssRpg.Domain.Activities.Services;

public static class ActivityExperienceCalculator
{
	public static int CalculateReward(TimeSpan duration, decimal experiencePerHour)
	{
		if (duration <= TimeSpan.Zero)
			throw new ActivityException("A duração da atividade deve ser maior que zero.");

		if (experiencePerHour <= 0)
			throw new ActivityException("A experiência por hora deve ser maior que zero.");

		decimal durationInHours = (decimal)duration.TotalHours;

		decimal baseExperience =
			durationInHours * experiencePerHour;

		decimal durationMultiplier =
			GetDurationMultiplier(duration);

		decimal finalExperience =
			baseExperience * durationMultiplier;

		return (int)Math.Ceiling(finalExperience);
	}
	//TODO Avaliar escalabilidade XP 
	public static decimal GetDurationMultiplier(TimeSpan duration)
	{
		if (duration <= TimeSpan.Zero)
			throw new ActivityException("A duração da atividade deve ser maior que zero.");

		if (duration >= TimeSpan.FromHours(12))
			return 1.20m;

		if (duration >= TimeSpan.FromHours(8))
			return 1.15m;

		if (duration >= TimeSpan.FromHours(4))
			return 1.10m;

		if (duration >= TimeSpan.FromHours(2))
			return 1.05m;

		return 1.00m;
	}
}