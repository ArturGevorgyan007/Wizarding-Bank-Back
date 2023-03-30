using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatetomaxvalue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "business",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    business_name = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    username = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    password = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    bin = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    address = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    wallet = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    business_type = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__business__3213E83FD089B494", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    full_name = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    username = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    password = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    address = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    email = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    wallet = table.Column<decimal>(type: "decimal(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__users__3213E83F555EE9F3", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "loans",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    amount = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    business_id = table.Column<int>(type: "int", nullable: true),
                    interest_rate = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    date_loaned = table.Column<DateTime>(type: "datetime", nullable: true),
                    loan_paid = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__loans__3213E83F8C2F8E66", x => x.id);
                    table.ForeignKey(
                        name: "FK__loans__business___6C190EBB",
                        column: x => x.business_id,
                        principalTable: "business",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    account_number = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    routing_number = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    business_id = table.Column<int>(type: "int", nullable: true),
                    balance = table.Column<decimal>(type: "decimal(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__accounts__3213E83FDE898024", x => x.id);
                    table.ForeignKey(
                        name: "FK__accounts__busine__656C112C",
                        column: x => x.business_id,
                        principalTable: "business",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__accounts__user_i__6477ECF3",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "cards",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    card_number = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    business_id = table.Column<int>(type: "int", nullable: true),
                    expiry_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    cvv = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    balance = table.Column<decimal>(type: "decimal(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__cards__3213E83F8065BDE1", x => x.id);
                    table.ForeignKey(
                        name: "FK__cards__business___693CA210",
                        column: x => x.business_id,
                        principalTable: "business",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__cards__user_id__68487DD7",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    amount = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    card_id = table.Column<int>(type: "int", nullable: true),
                    account_id = table.Column<int>(type: "int", nullable: true),
                    loan_id = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    description = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__transact__3213E83FC82F76D6", x => x.id);
                    table.ForeignKey(
                        name: "FK__transacti__accou__6FE99F9F",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__transacti__card___6EF57B66",
                        column: x => x.card_id,
                        principalTable: "cards",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__transacti__loan___70DDC3D8",
                        column: x => x.loan_id,
                        principalTable: "loans",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_accounts_business_id",
                table: "accounts",
                column: "business_id");

            migrationBuilder.CreateIndex(
                name: "IX_accounts_user_id",
                table: "accounts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UQ__business__AB6E616497C24BE0",
                table: "business",
                column: "email",
                unique: true,
                filter: "[email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__business__F3DBC57216CEA674",
                table: "business",
                column: "username",
                unique: true,
                filter: "[username] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_cards_business_id",
                table: "cards",
                column: "business_id");

            migrationBuilder.CreateIndex(
                name: "IX_cards_user_id",
                table: "cards",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_loans_business_id",
                table: "loans",
                column: "business_id");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_account_id",
                table: "transactions",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_card_id",
                table: "transactions",
                column: "card_id");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_loan_id",
                table: "transactions",
                column: "loan_id");

            migrationBuilder.CreateIndex(
                name: "UQ__users__AB6E6164E25936DC",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__users__F3DBC57215D5AFFC",
                table: "users",
                column: "username",
                unique: true,
                filter: "[username] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "cards");

            migrationBuilder.DropTable(
                name: "loans");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "business");
        }
    }
}
