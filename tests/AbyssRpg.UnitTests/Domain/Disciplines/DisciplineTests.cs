using AbyssRpg.Domain.Disciplines.Entities;
using AbyssRpg.Domain.Disciplines.Enums;
using AbyssRpg.Domain.Disciplines.Exceptions;

namespace AbyssRpg.UnitTests.Domain.Disciplines;

public sealed class DisciplineTests
{
	[Fact]
	public void Create_ShouldCreateDisciplineWithInitialValues()
	{
		Guid characterId = Guid.NewGuid();

		Discipline discipline = Discipline.Create(
			characterId,
			DisciplineType.Occultism
		);

		Assert.NotEqual(Guid.Empty, discipline.Id);
		Assert.Equal(characterId, discipline.CharacterId);
		Assert.Equal(DisciplineType.Occultism, discipline.Type);
		Assert.Equal(1, discipline.Level);
		Assert.Equal(0, discipline.Experience);
		Assert.Equal(20, discipline.GetExperienceRequiredForNextLevel());
	}

	[Fact]
	public void Create_ShouldThrowException_WhenCharacterIdIsEmpty()
	{
		DisciplineException exception =
			Assert.Throws<DisciplineException>(
				() => Discipline.Create(
					Guid.Empty,
					DisciplineType.Occultism
				)
			);

		Assert.Equal(
			"O identificador do personagem é obrigatório.",
			exception.Message
		);
	}

	[Fact]
	public void GainExperience_ShouldIncreaseCurrentExperience()
	{
		Discipline discipline = CreateDiscipline();

		discipline.GainExperience(8);

		Assert.Equal(1, discipline.Level);
		Assert.Equal(8, discipline.Experience);
		Assert.Equal(12, discipline.GetRemainingExperienceForNextLevel());
	}

	[Fact]
	public void GainExperience_ShouldLevelUpDiscipline()
	{
		Discipline discipline = CreateDiscipline();

		discipline.GainExperience(20);

		Assert.Equal(2, discipline.Level);
		Assert.Equal(0, discipline.Experience);
		Assert.Equal(30, discipline.GetExperienceRequiredForNextLevel());
	}

	[Fact]
	public void GainExperience_ShouldPreserveRemainingExperience()
	{
		Discipline discipline = CreateDiscipline();

		discipline.GainExperience(25);

		Assert.Equal(2, discipline.Level);
		Assert.Equal(5, discipline.Experience);
	}

	[Fact]
	public void GainExperience_ShouldAllowMultipleLevelUps()
	{
		Discipline discipline = CreateDiscipline();

		discipline.GainExperience(100);

		/*
         * Nível 1 → 2: custa 20
         * Nível 2 → 3: custa 30
         * Nível 3 → 4: custa 42
         *
         * Total gasto: 92
         * XP restante: 8
         */
		Assert.Equal(4, discipline.Level);
		Assert.Equal(8, discipline.Experience);
	}

	[Theory]
	[InlineData(0)]
	[InlineData(-1)]
	public void GainExperience_ShouldThrowException_WhenAmountIsInvalid(
		int amount)
	{
		Discipline discipline = CreateDiscipline();

		Assert.Throws<DisciplineException>(
			() => discipline.GainExperience(amount)
		);
	}

	private static Discipline CreateDiscipline()
	{
		return Discipline.Create(
			Guid.NewGuid(),
			DisciplineType.Occultism
		);
	}
}