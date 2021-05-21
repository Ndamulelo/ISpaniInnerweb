using Microsoft.EntityFrameworkCore.Migrations;

namespace ISpaniInnerweb.Infrastructure.Migrations
{
    public partial class UpdateSalaryDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "SalaryTo",
                table: "JobAdvert",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<long>(
                name: "SalaryFrom",
                table: "JobAdvert",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SalaryTo",
                table: "JobAdvert",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<decimal>(
                name: "SalaryFrom",
                table: "JobAdvert",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
