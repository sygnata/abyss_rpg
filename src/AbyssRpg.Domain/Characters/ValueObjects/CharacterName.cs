using AbyssRpg.Domain.Characters.Exceptions;

namespace AbyssRpg.Domain.Characters.ValueObjects;

public sealed record CharacterName
{
	public const int MinimumLength = 3;
	public const int MaximumLength = 30;

	private CharacterName(string value)
	{
		Value = value;
	}

	public string Value { get; }

	public static CharacterName Create(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new CharacterException("O nome do personagem é obrigatório.");

		string normalizedName = name.Trim();

		if (normalizedName.Length < MinimumLength)
			throw new CharacterException($"O nome do personagem deve possuir pelo menos {MinimumLength} caracteres.");

		if (normalizedName.Length > MaximumLength)
			throw new CharacterException( $"O nome do personagem deve possuir no máximo {MaximumLength} caracteres." );

		return new CharacterName(normalizedName);
	}

	public override string ToString()
	{
		return Value;
	}
}