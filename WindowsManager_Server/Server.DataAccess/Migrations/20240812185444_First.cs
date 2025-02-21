using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class First : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "host",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hostname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("host_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "publisher",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("publisher_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "software",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    version = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    publisherid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("software_pkey", x => x.id);
                    table.ForeignKey(
                        name: "software_publisherid_fkey",
                        column: x => x.publisherid,
                        principalTable: "publisher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "host_software",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    added = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    hostid = table.Column<int>(type: "integer", nullable: false),
                    softwareid = table.Column<int>(type: "integer", nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("host_software_pkey", x => x.id);
                    table.ForeignKey(
                        name: "host_software_hostid_fkey",
                        column: x => x.hostid,
                        principalTable: "host",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "host_software_softwareid_fkey",
                        column: x => x.softwareid,
                        principalTable: "software",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "host_hostname_key",
                table: "host",
                column: "hostname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_host_software_hostid",
                table: "host_software",
                column: "hostid");

            migrationBuilder.CreateIndex(
                name: "IX_host_software_softwareid",
                table: "host_software",
                column: "softwareid");

            migrationBuilder.CreateIndex(
                name: "publisher_title_key",
                table: "publisher",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_software_publisherid",
                table: "software",
                column: "publisherid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "host_software");

            migrationBuilder.DropTable(
                name: "host");

            migrationBuilder.DropTable(
                name: "software");

            migrationBuilder.DropTable(
                name: "publisher");
        }
    }
}
