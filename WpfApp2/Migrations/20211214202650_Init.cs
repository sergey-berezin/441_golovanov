using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WpfApp2.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImageBlob",
                columns: table => new
                {
                    ImageBlobId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Img = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageBlob", x => x.ImageBlobId);
                });

            migrationBuilder.CreateTable(
                name: "images",
                columns: table => new
                {
                    ImageId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImageHash = table.Column<int>(nullable: false),
                    BLOBImageBlobId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_images", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_images_ImageBlob_BLOBImageBlobId",
                        column: x => x.BLOBImageBlobId,
                        principalTable: "ImageBlob",
                        principalColumn: "ImageBlobId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "boxes",
                columns: table => new
                {
                    BoxId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Label = table.Column<string>(nullable: true),
                    x1 = table.Column<float>(nullable: false),
                    x2 = table.Column<float>(nullable: false),
                    x3 = table.Column<float>(nullable: false),
                    x4 = table.Column<float>(nullable: false),
                    Confidence = table.Column<float>(nullable: false),
                    ImageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_boxes", x => x.BoxId);
                    table.ForeignKey(
                        name: "FK_boxes_images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "images",
                        principalColumn: "ImageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_boxes_ImageId",
                table: "boxes",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_images_BLOBImageBlobId",
                table: "images",
                column: "BLOBImageBlobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "boxes");

            migrationBuilder.DropTable(
                name: "images");

            migrationBuilder.DropTable(
                name: "ImageBlob");
        }
    }
}
