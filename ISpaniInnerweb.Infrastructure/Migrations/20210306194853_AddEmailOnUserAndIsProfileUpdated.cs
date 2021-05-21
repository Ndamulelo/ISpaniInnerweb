using Microsoft.EntityFrameworkCore.Migrations;

namespace ISpaniInnerweb.Infrastructure.Migrations
{
    public partial class AddEmailOnUserAndIsProfileUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Education_JobSeeker_JobSeekerId",
                table: "Education");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSeekerJobApplications_JobSeeker_JobSeekerId",
                table: "JobSeekerJobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSeekerLanguages_JobSeeker_JobSeekerId",
                table: "JobSeekerLanguages");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSeekerSkills_JobSeeker_JobSeekerId",
                table: "JobSeekerSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkExperience_JobSeeker_JobSeekerId",
                table: "WorkExperience");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobSeeker",
                table: "JobSeeker");

            migrationBuilder.RenameTable(
                name: "JobSeeker",
                newName: "jobseeker");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsProfileUpdated",
                table: "jobseeker",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SalaryFrom",
                table: "JobAdvert",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SalaryTo",
                table: "JobAdvert",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_jobseeker",
                table: "jobseeker",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Education_jobseeker_JobSeekerId",
                table: "Education",
                column: "JobSeekerId",
                principalTable: "jobseeker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobSeekerJobApplications_jobseeker_JobSeekerId",
                table: "JobSeekerJobApplications",
                column: "JobSeekerId",
                principalTable: "jobseeker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobSeekerLanguages_jobseeker_JobSeekerId",
                table: "JobSeekerLanguages",
                column: "JobSeekerId",
                principalTable: "jobseeker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobSeekerSkills_jobseeker_JobSeekerId",
                table: "JobSeekerSkills",
                column: "JobSeekerId",
                principalTable: "jobseeker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkExperience_jobseeker_JobSeekerId",
                table: "WorkExperience",
                column: "JobSeekerId",
                principalTable: "jobseeker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Education_jobseeker_JobSeekerId",
                table: "Education");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSeekerJobApplications_jobseeker_JobSeekerId",
                table: "JobSeekerJobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSeekerLanguages_jobseeker_JobSeekerId",
                table: "JobSeekerLanguages");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSeekerSkills_jobseeker_JobSeekerId",
                table: "JobSeekerSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkExperience_jobseeker_JobSeekerId",
                table: "WorkExperience");

            migrationBuilder.DropPrimaryKey(
                name: "PK_jobseeker",
                table: "jobseeker");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsProfileUpdated",
                table: "jobseeker");

            migrationBuilder.DropColumn(
                name: "SalaryFrom",
                table: "JobAdvert");

            migrationBuilder.DropColumn(
                name: "SalaryTo",
                table: "JobAdvert");

            migrationBuilder.RenameTable(
                name: "jobseeker",
                newName: "JobSeeker");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobSeeker",
                table: "JobSeeker",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Education_JobSeeker_JobSeekerId",
                table: "Education",
                column: "JobSeekerId",
                principalTable: "JobSeeker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobSeekerJobApplications_JobSeeker_JobSeekerId",
                table: "JobSeekerJobApplications",
                column: "JobSeekerId",
                principalTable: "JobSeeker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobSeekerLanguages_JobSeeker_JobSeekerId",
                table: "JobSeekerLanguages",
                column: "JobSeekerId",
                principalTable: "JobSeeker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobSeekerSkills_JobSeeker_JobSeekerId",
                table: "JobSeekerSkills",
                column: "JobSeekerId",
                principalTable: "JobSeeker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkExperience_JobSeeker_JobSeekerId",
                table: "WorkExperience",
                column: "JobSeekerId",
                principalTable: "JobSeeker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
