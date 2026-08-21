using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbyssRpg.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueCharacterName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(
	            """
                CREATE UNIQUE INDEX "IX_characters_name_unique"
                ON characters (LOWER(name));
                """
            );
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(
	            """
                DROP INDEX IF EXISTS "IX_characters_name_unique";
                """
            );
		}
    }
}
