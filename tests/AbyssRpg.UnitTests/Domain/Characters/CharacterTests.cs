using AbyssRpg.Domain.Characters.Entities;
using AbyssRpg.Domain.Characters.Exceptions;
using AbyssRpg.Domain.Disciplines.Entities;
using AbyssRpg.Domain.Disciplines.Enums;

namespace AbyssRpg.UnitTests.Domain.Characters;

public sealed class CharacterTests
{
	#region Character Entity 
	[Fact]
	public void Create_ShouldCreateCharacterWithInitialValues()
	{
		Character character = Character.Create("Abraham Carter");

		Assert.NotEqual(Guid.Empty, character.Id);
		Assert.Equal("Abraham Carter", character.Name);
		Assert.Equal(1, character.Level);
		Assert.Equal(0, character.Experience);
		Assert.Equal(100, character.MaximumHealth);
		Assert.Equal(100, character.CurrentHealth);
		Assert.True(character.IsAlive());
		Assert.Equal(6, character.Disciplines.Count);
	}

	[Fact]
	public void Create_ShouldTrimCharacterName()
	{
		Character character = Character.Create("  Abraham Carter  ");

		Assert.Equal("Abraham Carter", character.Name);
	}

	[Fact]
	public void Create_ShouldThrowException_WhenNameIsEmpty()
	{
		CharacterException exception = Assert.Throws<CharacterException>(
			() => Character.Create(string.Empty)
		);

		Assert.Equal(
			"O nome do personagem é obrigatório.",
			exception.Message
		);
	}

	[Fact]
	public void GainExperience_ShouldIncreaseExperience()
	{
		Character character = Character.Create("Abraham Carter");

		character.GainExperience(50);

		Assert.Equal(50, character.Experience);
		Assert.Equal(1, character.Level);
	}

	[Fact]
	public void GainExperience_ShouldLevelUpCharacter()
	{
		Character character = Character.Create("Abraham Carter");

		character.GainExperience(100);

		Assert.Equal(2, character.Level);
		Assert.Equal(0, character.Experience);
		Assert.Equal(110, character.MaximumHealth);
		Assert.Equal(110, character.CurrentHealth);
	}

	[Fact]
	public void GainExperience_ShouldPreserveRemainingExperience()
	{
		Character character = Character.Create("Abraham Carter");

		character.GainExperience(125);

		Assert.Equal(2, character.Level);
		Assert.Equal(25, character.Experience);
	}

	[Fact]
	public void ReceiveDamage_ShouldReduceCurrentHealth()
	{
		Character character = Character.Create("Abraham Carter");

		character.ReceiveDamage(30);

		Assert.Equal(70, character.CurrentHealth);
	}

	[Fact]
	public void ReceiveDamage_ShouldNotReduceHealthBelowZero()
	{
		Character character = Character.Create("Abraham Carter");

		character.ReceiveDamage(150);

		Assert.Equal(0, character.CurrentHealth);
		Assert.False(character.IsAlive());
	}

	[Fact]
	public void RestoreHealth_ShouldNotExceedMaximumHealth()
	{
		Character character = Character.Create("Abraham Carter");

		character.ReceiveDamage(30);
		character.RestoreHealth(50);

		Assert.Equal(100, character.CurrentHealth);
	}
	#endregion

	#region Character/Discipline Tests
	[Fact]
	public void Create_ShouldCreateAllInitialDisciplines()
	{
		Character character = Character.Create("Abraham Carter");

		Assert.Equal(6, character.Disciplines.Count);

		Assert.Contains(
			character.Disciplines,
			discipline => discipline.Type == DisciplineType.Precision
		);

		Assert.Contains(
			character.Disciplines,
			discipline => discipline.Type == DisciplineType.Knowledge
		);

		Assert.Contains(
			character.Disciplines,
			discipline => discipline.Type == DisciplineType.Occultism
		);

		Assert.Contains(
			character.Disciplines,
			discipline => discipline.Type == DisciplineType.MentalFortitude
		);

		Assert.Contains(
			character.Disciplines,
			discipline => discipline.Type == DisciplineType.Artificer
		);

		Assert.Contains(
			character.Disciplines,
			discipline => discipline.Type == DisciplineType.Consecration
		);
	}

	[Fact]
	public void Create_ShouldCreateDisciplinesWithInitialValues()
	{
		Character character = Character.Create("Abraham Carter");

		Assert.All(
			character.Disciplines,
			discipline =>
			{
				Assert.Equal(character.Id, discipline.CharacterId);
				Assert.Equal(1, discipline.Level);
				Assert.Equal(0, discipline.Experience);
			}
		);
	}

	[Fact]
	public void GetDiscipline_ShouldReturnRequestedDiscipline()
	{
		Character character = Character.Create("Abraham Carter");

		Discipline discipline = character.GetDiscipline(
			DisciplineType.Occultism
		);

		Assert.Equal(DisciplineType.Occultism, discipline.Type);
		Assert.Equal(character.Id, discipline.CharacterId);
	}

	[Fact]
	public void GainDisciplineExperience_ShouldIncreaseRequestedDisciplineExperience()
	{
		Character character = Character.Create("Abraham Carter");

		character.GainDisciplineExperience(
			DisciplineType.Occultism,
			10
		);

		Discipline occultism = character.GetDiscipline(
			DisciplineType.Occultism
		);

		Assert.Equal(1, occultism.Level);
		Assert.Equal(10, occultism.Experience);
	}

	[Fact]
	public void GainDisciplineExperience_ShouldLevelUpRequestedDiscipline()
	{
		Character character = Character.Create("Abraham Carter");

		character.GainDisciplineExperience(
			DisciplineType.Occultism,
			20
		);

		Discipline occultism = character.GetDiscipline(
			DisciplineType.Occultism
		);

		Assert.Equal(2, occultism.Level);
		Assert.Equal(0, occultism.Experience);
	}

	[Fact]
	public void GainDisciplineExperience_ShouldNotChangeOtherDisciplines()
	{
		Character character = Character.Create("Abraham Carter");

		character.GainDisciplineExperience(
			DisciplineType.Occultism,
			10
		);

		Discipline precision = character.GetDiscipline(
			DisciplineType.Precision
		);

		Assert.Equal(1, precision.Level);
		Assert.Equal(0, precision.Experience);
	}
	#endregion
}