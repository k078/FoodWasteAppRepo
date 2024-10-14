using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.Identity
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "c99c1184-c118-401b-923e-8f39cfa5fdc1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "a9ea6de6-ba4e-4582-ac69-8042facc42cb");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ccbd0caa-3346-4fd9-9863-5e911ebff479", "AQAAAAEAACcQAAAAEKPUipRiq0wCdqj7LfCgk++rs47P0NUKhl19uF8bn3Aiy97USqXxV/vz963nb1iB3A==", "fc51e33b-b402-4e04-8bc7-7121f82b3eb2" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "08330d74-7c23-4075-9bbf-9d381b37e399", "AQAAAAEAACcQAAAAEDrHcxAoVjDVzNz4Kw4JF4yjVkgZra+viD5oDENnjiGvk43eEiA0V/dQvx2xsKrikQ==", "5d29cc62-8e0d-4be9-9f5d-6817be9ebb34" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "e590ae3b-8c9f-4d60-aacb-07e4bbef45f5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "b097544b-6d50-41e1-88f2-0bd95feeb85b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "36092480-10db-48cc-ba4d-c31b3a3d72d6", "AQAAAAEAACcQAAAAEExgRKsadCjPwOdWvEKQ4iEewJwwUEkNZvRnWTiyQbMa1+t8HZbcn3K/Q1Z+cFVAjA==", "6783bea5-cc88-4175-b5e7-db7670de692e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6cd84c83-1a0c-4e46-be1a-3b61ff6d8b57", "AQAAAAEAACcQAAAAEDxGlg6mhGvBBVF+kI71ZzX7d0pFBHQvmwkS7mMTZSKJaPeRxmFbhNuyqYjJdpaHSQ==", "e62dab46-c0f7-4d95-988a-c2ef75a3dc64" });
        }
    }
}
