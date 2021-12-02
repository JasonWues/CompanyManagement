using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(16)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(32)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConsumableInfo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(32)", nullable: true),
                    CategoryId = table.Column<string>(type: "varchar(36)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(16)", nullable: false),
                    Specification = table.Column<string>(type: "varchar(32)", nullable: false),
                    Num = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(8)", nullable: false),
                    Money = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WarningNum = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DeleteTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumableInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConsumableRecord",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", nullable: false),
                    ConsumableId = table.Column<string>(type: "varchar(36)", nullable: false),
                    Num = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<string>(type: "varchar(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumableRecord", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentInfo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(16)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    LeaderId = table.Column<string>(type: "varchar(36)", nullable: false),
                    ParentId = table.Column<string>(type: "varchar(36)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DeleteTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuInfo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(16)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(16)", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    Href = table.Column<string>(type: "varchar(128)", nullable: true),
                    ParentId = table.Column<string>(type: "varchar(36)", nullable: true),
                    Icon = table.Column<string>(type: "varchar(32)", nullable: true),
                    Target = table.Column<string>(type: "varchar(16)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DeleteTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleInfo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", nullable: false),
                    RoleName = table.Column<string>(type: "varchar(16)", nullable: false),
                    Description = table.Column<string>(type: "varchar(36)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DeleteTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RRoleInfoMenuInfo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", nullable: false),
                    RoleId = table.Column<string>(type: "varchar(36)", nullable: true),
                    MenuId = table.Column<string>(type: "varchar(36)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RRoleInfoMenuInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RUserInfoRoleInfo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(36)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(36)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RUserInfoRoleInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", nullable: false),
                    Account = table.Column<string>(type: "varchar(16)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(16)", nullable: false),
                    PhoneNum = table.Column<string>(type: "varchar(16)", nullable: true),
                    Email = table.Column<string>(type: "varchar(32)", nullable: true),
                    DepartmentId = table.Column<string>(type: "varchar(36)", nullable: true),
                    Sex = table.Column<int>(type: "int", nullable: false),
                    PassWord = table.Column<string>(type: "varchar(32)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DeleteTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isAdmin = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "ConsumableInfo");

            migrationBuilder.DropTable(
                name: "ConsumableRecord");

            migrationBuilder.DropTable(
                name: "DepartmentInfo");

            migrationBuilder.DropTable(
                name: "MenuInfo");

            migrationBuilder.DropTable(
                name: "RoleInfo");

            migrationBuilder.DropTable(
                name: "RRoleInfoMenuInfo");

            migrationBuilder.DropTable(
                name: "RUserInfoRoleInfo");

            migrationBuilder.DropTable(
                name: "UserInfo");
        }
    }
}
