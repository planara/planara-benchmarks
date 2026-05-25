using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planara.Benchmarks.Data.Migrations
{
    /// <inheritdoc />
    public partial class BenchmarkRuns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BenchmarkRuns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DurationMs = table.Column<double>(type: "double precision", nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    UserAgent = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    DevicePixelRatio = table.Column<double>(type: "double precision", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenchmarkRuns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BenchmarkTestResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RunId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ErrorMessage = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    DurationMs = table.Column<double>(type: "double precision", nullable: false),
                    Frames = table.Column<int>(type: "integer", nullable: false),
                    AverageFps = table.Column<double>(type: "double precision", nullable: false),
                    MinFps = table.Column<double>(type: "double precision", nullable: false),
                    AverageFrameTime = table.Column<double>(type: "double precision", nullable: false),
                    MaxFrameTime = table.Column<double>(type: "double precision", nullable: false),
                    ObjectsCount = table.Column<int>(type: "integer", nullable: false),
                    DrawCalls = table.Column<int>(type: "integer", nullable: false),
                    Triangles = table.Column<int>(type: "integer", nullable: false),
                    Geometries = table.Column<int>(type: "integer", nullable: false),
                    Textures = table.Column<int>(type: "integer", nullable: false),
                    MemoryUsedMb = table.Column<double>(type: "double precision", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    History = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenchmarkTestResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BenchmarkTestResults_BenchmarkRuns_RunId",
                        column: x => x.RunId,
                        principalTable: "BenchmarkRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkRuns_UserId",
                table: "BenchmarkRuns",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkRuns_UserId_CreatedAt",
                table: "BenchmarkRuns",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkTestResults_RunId",
                table: "BenchmarkTestResults",
                column: "RunId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BenchmarkTestResults");

            migrationBuilder.DropTable(
                name: "BenchmarkRuns");
        }
    }
}
