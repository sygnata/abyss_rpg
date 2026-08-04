using AbyssRpg.Domain.Characters.Entities;
using AbyssRpg.Domain.Characters.Exceptions;

namespace AbyssRpg.UnitTests.Domain.Characters;

public sealed class CharacterTests
{
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
}