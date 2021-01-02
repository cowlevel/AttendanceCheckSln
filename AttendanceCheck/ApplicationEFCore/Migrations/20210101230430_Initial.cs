using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AttendanceCheck.ApplicationEFCore.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUsers",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityGUID = table.Column<string>(maxLength: 450, nullable: true),
                    LastName = table.Column<string>(maxLength: 70, nullable: false),
                    FirstName = table.Column<string>(maxLength: 70, nullable: false),
                    NotifNoOfAbsentInMonth = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsers", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    AttendanceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserUserId = table.Column<int>(nullable: true),
                    AttDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.AttendanceId);
                    table.ForeignKey(
                        name: "FK_Attendances_ApplicationUsers_AppUserUserId",
                        column: x => x.AppUserUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserUserId = table.Column<int>(nullable: true),
                    GroupName = table.Column<string>(maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_Groups_ApplicationUsers_AppUserUserId",
                        column: x => x.AppUserUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    PersonId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonGroupGroupId = table.Column<int>(nullable: true),
                    LastName = table.Column<string>(maxLength: 70, nullable: false),
                    FirstName = table.Column<string>(maxLength: 70, nullable: false),
                    ContactNo = table.Column<string>(maxLength: 22, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.PersonId);
                    table.ForeignKey(
                        name: "FK_People_Groups_PersonGroupGroupId",
                        column: x => x.PersonGroupGroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceDetails",
                columns: table => new
                {
                    AttendanceDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttendanceId = table.Column<int>(nullable: true),
                    PersonAttDetailPersonId = table.Column<int>(nullable: true),
                    AttStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceDetails", x => x.AttendanceDetailId);
                    table.ForeignKey(
                        name: "FK_AttendanceDetails_Attendances_AttendanceId",
                        column: x => x.AttendanceId,
                        principalTable: "Attendances",
                        principalColumn: "AttendanceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttendanceDetails_People_PersonAttDetailPersonId",
                        column: x => x.PersonAttDetailPersonId,
                        principalTable: "People",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceNotifications",
                columns: table => new
                {
                    AttNotificationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserUserId = table.Column<int>(nullable: true),
                    PersonInNotifPersonId = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    NotifNoOfAbsentCount = table.Column<int>(nullable: false),
                    Noted = table.Column<bool>(nullable: false),
                    NotedDate = table.Column<DateTime>(nullable: false),
                    ActionMade = table.Column<string>(maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceNotifications", x => x.AttNotificationId);
                    table.ForeignKey(
                        name: "FK_AttendanceNotifications_ApplicationUsers_AppUserUserId",
                        column: x => x.AppUserUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttendanceNotifications_People_PersonInNotifPersonId",
                        column: x => x.PersonInNotifPersonId,
                        principalTable: "People",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceDetails_AttendanceId",
                table: "AttendanceDetails",
                column: "AttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceDetails_PersonAttDetailPersonId",
                table: "AttendanceDetails",
                column: "PersonAttDetailPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceNotifications_AppUserUserId",
                table: "AttendanceNotifications",
                column: "AppUserUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceNotifications_PersonInNotifPersonId",
                table: "AttendanceNotifications",
                column: "PersonInNotifPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_AppUserUserId",
                table: "Attendances",
                column: "AppUserUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_AppUserUserId",
                table: "Groups",
                column: "AppUserUserId");

            migrationBuilder.CreateIndex(
                name: "IX_People_PersonGroupGroupId",
                table: "People",
                column: "PersonGroupGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceDetails");

            migrationBuilder.DropTable(
                name: "AttendanceNotifications");

            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "ApplicationUsers");
        }
    }
}
