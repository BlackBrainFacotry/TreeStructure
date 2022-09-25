using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetCoreWebApi.Data.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    NodeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nodes_Nodes_NodeId",
                        column: x => x.NodeId,
                        principalTable: "Nodes",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "Name", "NodeId", "ParentId" },
                values: new object[,]
                {
                    { 1, "example element 1a", null, null },
                    { 2, "example element 1b", null, null },
                    { 3, "example element 1c", null, null },
                    { 4, "example element 1d", null, null },
                    { 5, "example element 2a", null, 1 },
                    { 6, "example element 2b", null, 2 },
                    { 7, "example element 2c", null, 3 },
                    { 8, "example element 2d", null, 4 },
                    { 9, "example element 3a", null, 5 },
                    { 10, "example element 3b", null, 5 },
                    { 11, "example element 3c", null, 6 },
                    { 12, "example element 3d", null, 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_NodeId",
                table: "Nodes",
                column: "NodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nodes");
        }
    }
}
