using AbyssRpg.Domain.Disciplines.Exceptions;

namespace AbyssRpg.Domain.Disciplines.Services;

public static class DisciplineExperienceCalculator
{
	/*TODO Valores provisórios.
     *
     * Eles serão movidos posteriormente para uma configuração
     * de balanceamento armazenada fora do domínio.
     */
	private const decimal BaseExperience = 20m;
	private const decimal LinearGrowth = 8m;
	private const decimal QuadraticGrowth = 1.5m;

	public static int CalculateRequiredExperience(int currentLevel)
	{
		if (currentLevel <= 0)
		{
			throw new DisciplineException(
				"O nível atual da disciplina deve ser maior que zero."
			);
		}

		decimal normalizedLevel = currentLevel - 1;

		decimal requiredExperience =
			BaseExperience
			+ (LinearGrowth * normalizedLevel)
			+ (QuadraticGrowth * normalizedLevel * normalizedLevel);

		return (int)Math.Ceiling(requiredExperience);
	}
}