using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class addedscoresintoAccounttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "InfluenceScore",
                table: "Accounts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LikeHIndexScore",
                table: "Accounts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ReTweetHIndexScore",
                table: "Accounts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TweetsCredibilityScore",
                table: "Accounts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "UserReputationScore",
                table: "Accounts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InfluenceScore",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "LikeHIndexScore",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ReTweetHIndexScore",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "TweetsCredibilityScore",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "UserReputationScore",
                table: "Accounts");
        }
    }
}
