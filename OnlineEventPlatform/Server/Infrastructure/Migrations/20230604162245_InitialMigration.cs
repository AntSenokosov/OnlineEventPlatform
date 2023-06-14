using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "event_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "eventplatform_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "eventspeakers_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "platform_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "speaker_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "type_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "user_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "userevent_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "MailTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Html = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Css = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultTemplate = table.Column<bool>(type: "bit", nullable: false),
                    WelcomeTemplate = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeetingPlatforms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingPlatforms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Speakers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LongDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Speakers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Salt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    GoogleAuthKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsSuperAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsGoogleAuthEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OnlineEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    AboutEvent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Photo = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnlineEvents_TypeOfEvents_TypeId",
                        column: x => x.TypeId,
                        principalTable: "TypeOfEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventPlatforms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    PlatformId = table.Column<int>(type: "int", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventPlatforms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventPlatforms_MeetingPlatforms_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "MeetingPlatforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventPlatforms_OnlineEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "OnlineEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventSpeakers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    SpeakerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSpeakers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventSpeakers_OnlineEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "OnlineEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventSpeakers_Speakers_SpeakerId",
                        column: x => x.SpeakerId,
                        principalTable: "Speakers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    OnlineEventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserEvents_OnlineEvents_OnlineEventId",
                        column: x => x.OnlineEventId,
                        principalTable: "OnlineEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserEvents_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventPlatforms_EventId",
                table: "EventPlatforms",
                column: "EventId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventPlatforms_PlatformId",
                table: "EventPlatforms",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSpeakers_EventId",
                table: "EventSpeakers",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSpeakers_SpeakerId",
                table: "EventSpeakers",
                column: "SpeakerId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineEvents_TypeId",
                table: "OnlineEvents",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEvents_OnlineEventId",
                table: "UserEvents",
                column: "OnlineEventId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEvents_UserId",
                table: "UserEvents",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventPlatforms");

            migrationBuilder.DropTable(
                name: "EventSpeakers");

            migrationBuilder.DropTable(
                name: "MailTemplates");

            migrationBuilder.DropTable(
                name: "UserEvents");

            migrationBuilder.DropTable(
                name: "MeetingPlatforms");

            migrationBuilder.DropTable(
                name: "Speakers");

            migrationBuilder.DropTable(
                name: "OnlineEvents");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "TypeOfEvents");

            migrationBuilder.DropSequence(
                name: "event_hilo");

            migrationBuilder.DropSequence(
                name: "eventplatform_hilo");

            migrationBuilder.DropSequence(
                name: "eventspeakers_hilo");

            migrationBuilder.DropSequence(
                name: "platform_hilo");

            migrationBuilder.DropSequence(
                name: "speaker_hilo");

            migrationBuilder.DropSequence(
                name: "type_hilo");

            migrationBuilder.DropSequence(
                name: "user_hilo");

            migrationBuilder.DropSequence(
                name: "userevent_hilo");
        }
    }
}
