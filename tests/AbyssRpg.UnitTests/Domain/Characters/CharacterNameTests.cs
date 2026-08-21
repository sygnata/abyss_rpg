using AbyssRpg.Domain.Characters.Exceptions;
using AbyssRpg.Domain.Characters.ValueObjects;

namespace AbyssRpg.UnitTests.Domain.Characters;

public sealed class CharacterNameTests
{
	[Fact]
	public void Create_ShouldCreateCharacterName()
	{
		CharacterName name = CharacterName.Create("Abraham Carter");

		Assert.Equal( "Abraham Carter", name.Value );
	}

	[Fact]
	public void Create_ShouldTrimName()
	{
		CharacterName name = CharacterName.Create( "  Abraham Carter  " );

		Assert.Equal( "Abraham Carter", name.Value );
	}

	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	[InlineData("   ")]
	public void Create_ShouldThrowException_WhenNameIsEmpty(
		string invalidName)
	{
		Assert.Throws<CharacterException>(
			() => CharacterName.Create(
				invalidName
			)
		);
	}

	[Fact]
	public void Create_ShouldThrowException_WhenNameIsTooShort()
	{
		Assert.Throws<CharacterException>( () => CharacterName.Create("AB") );
	}

	[Fact]
	public void Create_ShouldThrowException_WhenNameIsTooLong()
	{
		string name = new( 'A', CharacterName.MaximumLength + 1 );

		Assert.Throws<CharacterException>( () => CharacterName.Create(name) );
	}
}