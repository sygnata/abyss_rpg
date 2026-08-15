using System;
using System.Collections.Generic;
using System.Text;

namespace AbyssRpg.Application.Characters.Create
{
	public sealed record CreateCharacterResult(
		Guid Id,
		string Name,
		int Level,
		int CurrentHealth,
		int MaximumHealth
	);
}
