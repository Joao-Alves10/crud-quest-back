using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuestAPI.Migrations
{
    public partial class CriacaoTabelasQuest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alternativa",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlternativaCorreta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlternativaErrada1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlternativaErrada2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlternativaErrada3 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alternativa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tema",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DescTema = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tema", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pergunta",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DescPergunta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemaPerguntaId = table.Column<long>(type: "bigint", nullable: true),
                    TemaId = table.Column<long>(type: "bigint", nullable: false),
                    AlternativaId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pergunta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pergunta_Alternativa_AlternativaId",
                        column: x => x.AlternativaId,
                        principalTable: "Alternativa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pergunta_Tema_TemaId",
                        column: x => x.TemaId,
                        principalTable: "Tema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pergunta_AlternativaId",
                table: "Pergunta",
                column: "AlternativaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pergunta_TemaId",
                table: "Pergunta",
                column: "TemaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pergunta");

            migrationBuilder.DropTable(
                name: "Alternativa");

            migrationBuilder.DropTable(
                name: "Tema");
        }
    }
}
