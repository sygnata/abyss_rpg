using AbyssRpg.Domain.Shared.Exceptions;

namespace AbyssRpg.Domain.Characters.Exceptions;

public sealed class CharacterException : DomainException
{
	public CharacterException(string message)
		: base(message)
	{
	}
}