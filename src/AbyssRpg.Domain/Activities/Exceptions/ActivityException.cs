using AbyssRpg.Domain.Shared.Exceptions;

namespace AbyssRpg.Domain.Activities.Exceptions;

public sealed class ActivityException : DomainException
{
	public ActivityException(string message)
		: base(message)
	{
	}
}