using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektZespolowy.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Coordinates = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedCost = table.Column<int>(type: "int", nullable: false),
                    AverageTimeSpent = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Places_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    VotesFor = table.Column<int>(type: "int", nullable: false),
                    VotesAgainst = table.Column<int>(type: "int", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StartHour = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StopHour = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransportType = table.Column<int>(type: "int", nullable: false),
                    Budget = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimesForPlaces = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trips_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommentPlace",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentPlace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentPlace_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommentPlace_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlaceAvailabilityTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    OpeningMonday = table.Column<int>(type: "int", nullable: false),
                    OpeningTuesday = table.Column<int>(type: "int", nullable: false),
                    OpeningWednesday = table.Column<int>(type: "int", nullable: false),
                    OpeningThursday = table.Column<int>(type: "int", nullable: false),
                    OpeningFriday = table.Column<int>(type: "int", nullable: false),
                    OpeningSaturday = table.Column<int>(type: "int", nullable: false),
                    OpeningSunday = table.Column<int>(type: "int", nullable: false),
                    ClosingMonday = table.Column<int>(type: "int", nullable: false),
                    ClosingTuesday = table.Column<int>(type: "int", nullable: false),
                    ClosingWednesday = table.Column<int>(type: "int", nullable: false),
                    ClosingThursday = table.Column<int>(type: "int", nullable: false),
                    ClosingFriday = table.Column<int>(type: "int", nullable: false),
                    ClosingSaturday = table.Column<int>(type: "int", nullable: false),
                    ClosingSunday = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceAvailabilityTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaceAvailabilityTimes_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlaceLikes",
                columns: table => new
                {
                    PlaceLikeId = table.Column<int>(type: "int", nullable: false),
                    UserLikeId = table.Column<int>(type: "int", nullable: false),
                    IsLike = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceLikes", x => new { x.PlaceLikeId, x.UserLikeId });
                    table.ForeignKey(
                        name: "FK_PlaceLikes_Places_PlaceLikeId",
                        column: x => x.PlaceLikeId,
                        principalTable: "Places",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlaceLikes_Users_UserLikeId",
                        column: x => x.UserLikeId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PostLikes",
                columns: table => new
                {
                    PostLikeId = table.Column<int>(type: "int", nullable: false),
                    UserLikeId = table.Column<int>(type: "int", nullable: false),
                    IsLike = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostLikes", x => new { x.PostLikeId, x.UserLikeId });
                    table.ForeignKey(
                        name: "FK_PostLikes_Posts_PostLikeId",
                        column: x => x.PostLikeId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PostLikes_Users_UserLikeId",
                        column: x => x.UserLikeId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlacesTrips",
                columns: table => new
                {
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    TripId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlacesTrips", x => new { x.PlaceId, x.TripId });
                    table.ForeignKey(
                        name: "FK_PlacesTrips_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlacesTrips_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TripLikes",
                columns: table => new
                {
                    TripLikeId = table.Column<int>(type: "int", nullable: false),
                    UserLikeId = table.Column<int>(type: "int", nullable: false),
                    IsLike = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripLikes", x => new { x.TripLikeId, x.UserLikeId });
                    table.ForeignKey(
                        name: "FK_TripLikes_Trips_TripLikeId",
                        column: x => x.TripLikeId,
                        principalTable: "Trips",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TripLikes_Users_UserLikeId",
                        column: x => x.UserLikeId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommentPlaceLikes",
                columns: table => new
                {
                    CommentPlaceLikeId = table.Column<int>(type: "int", nullable: false),
                    UserLikeId = table.Column<int>(type: "int", nullable: false),
                    IsLike = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentPlaceLikes", x => new { x.CommentPlaceLikeId, x.UserLikeId });
                    table.ForeignKey(
                        name: "FK_CommentPlaceLikes_CommentPlace_CommentPlaceLikeId",
                        column: x => x.CommentPlaceLikeId,
                        principalTable: "CommentPlace",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommentPlaceLikes_Users_UserLikeId",
                        column: x => x.UserLikeId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommentLikes",
                columns: table => new
                {
                    CommentLikeId = table.Column<int>(type: "int", nullable: false),
                    UserLikeId = table.Column<int>(type: "int", nullable: false),
                    IsLike = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentLikes", x => new { x.CommentLikeId, x.UserLikeId });
                    table.ForeignKey(
                        name: "FK_CommentLikes_Comments_CommentLikeId",
                        column: x => x.CommentLikeId,
                        principalTable: "Comments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommentLikes_Users_UserLikeId",
                        column: x => x.UserLikeId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentLikes_UserLikeId",
                table: "CommentLikes",
                column: "UserLikeId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentPlace_PlaceId",
                table: "CommentPlace",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentPlace_UserId",
                table: "CommentPlace",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentPlaceLikes_UserLikeId",
                table: "CommentPlaceLikes",
                column: "UserLikeId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaceAvailabilityTimes_PlaceId",
                table: "PlaceAvailabilityTimes",
                column: "PlaceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlaceLikes_UserLikeId",
                table: "PlaceLikes",
                column: "UserLikeId");

            migrationBuilder.CreateIndex(
                name: "IX_Places_UserId",
                table: "Places",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlacesTrips_TripId",
                table: "PlacesTrips",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_PostLikes_UserLikeId",
                table: "PostLikes",
                column: "UserLikeId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TripLikes_UserLikeId",
                table: "TripLikes",
                column: "UserLikeId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_UserId",
                table: "Trips",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentLikes");

            migrationBuilder.DropTable(
                name: "CommentPlaceLikes");

            migrationBuilder.DropTable(
                name: "PlaceAvailabilityTimes");

            migrationBuilder.DropTable(
                name: "PlaceLikes");

            migrationBuilder.DropTable(
                name: "PlacesTrips");

            migrationBuilder.DropTable(
                name: "PostLikes");

            migrationBuilder.DropTable(
                name: "TripLikes");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "CommentPlace");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
