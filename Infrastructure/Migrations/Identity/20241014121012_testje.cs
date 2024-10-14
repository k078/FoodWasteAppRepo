using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.Identity
{
    public partial class testje : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                columns: new[] { "ConcurrencyStamp", "Email", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "36092480-10db-48cc-ba4d-c31b3a3d72d6", "truus@avans.nl", "AQAAAAEAACcQAAAAEExgRKsadCjPwOdWvEKQ4iEewJwwUEkNZvRnWTiyQbMa1+t8HZbcn3K/Q1Z+cFVAjA==", "6783bea5-cc88-4175-b5e7-db7670de692e", "truus@avans.nl" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "Email", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "6cd84c83-1a0c-4e46-be1a-3b61ff6d8b57", "kalle@mail.com", "AQAAAAEAACcQAAAAEDxGlg6mhGvBBVF+kI71ZzX7d0pFBHQvmwkS7mMTZSKJaPeRxmFbhNuyqYjJdpaHSQ==", "e62dab46-c0f7-4d95-988a-c2ef75a3dc64", "kalle@mail.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "370dc991-5e22-4122-92e6-86393129662d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "c65ef49e-4d2f-45d9-9249-1b0c0bf71698");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "Email", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "a6b99bf6-12cd-4692-a62f-0392a9c3dfaa", "kalle@mail.com", "AQAAAAEAACcQAAAAECsCX6dFs+E8+4RZxWAId9VL2KG5Tuf7VW3JlwqizJwb6qRfUdpsEGo0LySWABG0KQ==", "d6809846-9923-4c47-b166-3c33a835bf4d", "kalle@mail.com" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "Email", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "6cec2011-b8d8-43c3-aed7-3f9ba076650f", "truus@avans.nl", "AQAAAAEAACcQAAAAEM0vpZR0oiL5drDlF3v1X9dH4xUiriXyJ08gErA8SFtvWfk2mJxbvzCOQ9RC8/BWDA==", "7750b912-0cd3-4277-95d0-ba3f68a5b3ac", "truus@avans.nl" });
        }
    }
}
