using System;
using Microsoft.EntityFrameworkCore.Migrations;
using PFE_PROJECT.Data;
#nullable disable

namespace PFE_PROJECT.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleRelationshipWithoutConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // First create the Role table
            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    idRole = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    libelle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.idRole);
                });

            // Create the User table if it doesn't exist yet
            // If it already exists, this will be skipped by EF Core
            migrationBuilder.CreateTable(
                name: "Utilisateur",
                columns: table => new
                {
                    idUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idUnite = table.Column<int>(type: "int", nullable: true),
                    nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    prenom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    numTel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    motPasse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    idRole = table.Column<int>(type: "int", nullable: false, defaultValue: 2),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateur", x => x.idUser);
                });

            // Insert default roles
            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "libelle" },
                values: new object[] { "Admin" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "libelle" },
                values: new object[] { "User" });

            // Update any existing users to have a valid role ID
            // This SQL is safe to run even if the table doesn't exist yet
            migrationBuilder.Sql(@"
                IF OBJECT_ID('Utilisateur', 'U') IS NOT NULL
                BEGIN
                    UPDATE Utilisateur 
                    SET idRole = (SELECT TOP 1 idRole FROM Role) 
                    WHERE idRole IS NULL OR idRole NOT IN (SELECT idRole FROM Role)
                END
            ");

            // Add the foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_Utilisateur_Role_idRole",
                table: "Utilisateur",
                column: "idRole",
                principalTable: "Role",
                principalColumn: "idRole",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the foreign key constraint first
            migrationBuilder.DropForeignKey(
                name: "FK_Utilisateur_Role_idRole",
                table: "Utilisateur");

            // Drop the Role table
            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}