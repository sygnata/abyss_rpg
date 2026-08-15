using AbyssRpg.Domain.Disciplines.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbyssRpg.Infrastructure.Persistence.Configurations;

public sealed class DisciplineConfiguration : IEntityTypeConfiguration<Discipline>
{
	public void Configure( EntityTypeBuilder<Discipline> builder)
	{
		builder.ToTable("disciplines");

		builder.HasKey(discipline => discipline.Id);

		builder.Property(discipline => discipline.Id)
			.HasColumnName("id");

		builder.Property(discipline => discipline.CharacterId)
			.HasColumnName("character_id")
			.IsRequired();

		builder.Property(discipline => discipline.Type)
			.HasColumnName("type")
			.HasConversion<int>()
			.IsRequired();

		builder.Property(discipline => discipline.Level)
			.HasColumnName("level")
			.IsRequired();

		builder.Property(discipline => discipline.Experience)
			.HasColumnName("experience")
			.IsRequired();

		builder.Property(discipline => discipline.CreatedAt)
			.HasColumnName("created_at")
			.IsRequired();

		builder.HasIndex(
				discipline => new
				{
					discipline.CharacterId,
					discipline.Type
				})
			.IsUnique();
	}
}