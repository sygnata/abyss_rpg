using AbyssRpg.Domain.Shared.Exceptions;

namespace AbyssRpg.Domain.Disciplines.Exceptions;

public sealed class DisciplineException : DomainException
{
	public DisciplineException(string message)
		: base(message)
	{
	}
}