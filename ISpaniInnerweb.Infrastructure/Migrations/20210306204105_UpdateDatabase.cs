using Microsoft.EntityFrameworkCore.Migrations;

namespace ISpaniInnerweb.Infrastructure.Migrations
{
    public partial class UpdateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SalaryTo",
                table: "JobAdvert",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<decimal>(
                name: "SalaryFrom",
                table: "JobAdvert",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "SalaryTo",
                table: "JobAdvert",
                type: "double",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "SalaryFrom",
                table: "JobAdvert",
                type: "double",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
