using AbyssRpg.Domain.Characters.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbyssRpg.Infrastructure.Persistence.Configurations;

public sealed class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
	public void Configure( EntityTypeBuilder<Character> builder)
	{
		builder.ToTable("characters");

		builder.HasKey(character => character.Id);

		builder.Property(character => character.Id)
			.HasColumnName("id");

		builder.Property(character => character.Name)
			.HasColumnName("name")
			.HasMaxLength(30)
			.IsRequired();

		builder.Property(character => character.Level)
			.HasColumnName("level")
			.IsRequired();

		builder.Property(character => character.Experience)
			.HasColumnName("experience")
			.IsRequired();

		builder.Property(character => character.CurrentHealth)
			.HasColumnName("current_health")
			.IsRequired();

		builder.Property(character => character.MaximumHealth)
			.HasColumnName("maximum_health")
			.IsRequired();

		builder.Property(character => character.CreatedAt)
			.HasColumnName("created_at")
			.IsRequired();

		builder.HasMany(character => character.Disciplines)
			.WithOne()
			.HasForeignKey(discipline => discipline.CharacterId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}