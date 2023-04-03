﻿// <auto-generated />
using System;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(WizardingBankDbContext))]
    [Migration("20230330205506_updatetomaxvalue")]
    partial class updatetomaxvalue
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DataAccess.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AccountNumber")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("account_number");

                    b.Property<decimal?>("Balance")
                        .HasColumnType("decimal(18, 0)")
                        .HasColumnName("balance");

                    b.Property<int?>("BusinessId")
                        .HasColumnType("int")
                        .HasColumnName("business_id");

                    b.Property<string>("RoutingNumber")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("routing_number");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("PK__accounts__3213E83FDE898024");

                    b.HasIndex("BusinessId");

                    b.HasIndex("UserId");

                    b.ToTable("accounts", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Business", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("address");

                    b.Property<string>("Bin")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("bin");

                    b.Property<string>("BusinessName")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("business_name");

                    b.Property<string>("BusinessType")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("business_type");

                    b.Property<string>("Email")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("email");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("password");

                    b.Property<string>("Username")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("username");

                    b.Property<decimal?>("Wallet")
                        .HasColumnType("decimal(18, 0)")
                        .HasColumnName("wallet");

                    b.HasKey("Id")
                        .HasName("PK__business__3213E83FD089B494");

                    b.HasIndex(new[] { "Email" }, "UQ__business__AB6E616497C24BE0")
                        .IsUnique()
                        .HasFilter("[email] IS NOT NULL");

                    b.HasIndex(new[] { "Username" }, "UQ__business__F3DBC57216CEA674")
                        .IsUnique()
                        .HasFilter("[username] IS NOT NULL");

                    b.ToTable("business", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal?>("Balance")
                        .HasColumnType("decimal(18, 0)")
                        .HasColumnName("balance");

                    b.Property<int?>("BusinessId")
                        .HasColumnType("int")
                        .HasColumnName("business_id");

                    b.Property<string>("CardNumber")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("card_number");

                    b.Property<string>("Cvv")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("cvv");

                    b.Property<DateTime?>("ExpiryDate")
                        .HasColumnType("datetime")
                        .HasColumnName("expiry_date");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("PK__cards__3213E83F8065BDE1");

                    b.HasIndex("BusinessId");

                    b.HasIndex("UserId");

                    b.ToTable("cards", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Loan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal?>("Amount")
                        .HasColumnType("decimal(18, 0)")
                        .HasColumnName("amount");

                    b.Property<int?>("BusinessId")
                        .HasColumnType("int")
                        .HasColumnName("business_id");

                    b.Property<DateTime?>("DateLoaned")
                        .HasColumnType("datetime")
                        .HasColumnName("date_loaned");

                    b.Property<decimal?>("InterestRate")
                        .HasColumnType("decimal(18, 0)")
                        .HasColumnName("interest_rate");

                    b.Property<DateTime?>("LoanPaid")
                        .HasColumnType("datetime")
                        .HasColumnName("loan_paid");

                    b.HasKey("Id")
                        .HasName("PK__loans__3213E83F8C2F8E66");

                    b.HasIndex("BusinessId");

                    b.ToTable("loans", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AccountId")
                        .HasColumnType("int")
                        .HasColumnName("account_id");

                    b.Property<decimal?>("Amount")
                        .HasColumnType("decimal(18, 0)")
                        .HasColumnName("amount");

                    b.Property<int?>("CardId")
                        .HasColumnType("int")
                        .HasColumnName("card_id");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("description");

                    b.Property<int?>("LoanId")
                        .HasColumnType("int")
                        .HasColumnName("loan_id");

                    b.HasKey("Id")
                        .HasName("PK__transact__3213E83FC82F76D6");

                    b.HasIndex("AccountId");

                    b.HasIndex("CardId");

                    b.HasIndex("LoanId");

                    b.ToTable("transactions", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("address");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("email");

                    b.Property<string>("FullName")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("full_name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("password");

                    b.Property<string>("Username")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("username");

                    b.Property<decimal?>("Wallet")
                        .HasColumnType("decimal(18, 0)")
                        .HasColumnName("wallet");

                    b.HasKey("Id")
                        .HasName("PK__users__3213E83F555EE9F3");

                    b.HasIndex(new[] { "Email" }, "UQ__users__AB6E6164E25936DC")
                        .IsUnique();

                    b.HasIndex(new[] { "Username" }, "UQ__users__F3DBC57215D5AFFC")
                        .IsUnique()
                        .HasFilter("[username] IS NOT NULL");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Account", b =>
                {
                    b.HasOne("DataAccess.Entities.Business", "Business")
                        .WithMany("Accounts")
                        .HasForeignKey("BusinessId")
                        .HasConstraintName("FK__accounts__busine__656C112C");

                    b.HasOne("DataAccess.Entities.User", "User")
                        .WithMany("Accounts")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__accounts__user_i__6477ECF3");

                    b.Navigation("Business");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Entities.Card", b =>
                {
                    b.HasOne("DataAccess.Entities.Business", "Business")
                        .WithMany("Cards")
                        .HasForeignKey("BusinessId")
                        .HasConstraintName("FK__cards__business___693CA210");

                    b.HasOne("DataAccess.Entities.User", "User")
                        .WithMany("Cards")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__cards__user_id__68487DD7");

                    b.Navigation("Business");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Entities.Loan", b =>
                {
                    b.HasOne("DataAccess.Entities.Business", "Business")
                        .WithMany("Loans")
                        .HasForeignKey("BusinessId")
                        .HasConstraintName("FK__loans__business___6C190EBB");

                    b.Navigation("Business");
                });

            modelBuilder.Entity("DataAccess.Entities.Transaction", b =>
                {
                    b.HasOne("DataAccess.Entities.Account", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountId")
                        .HasConstraintName("FK__transacti__accou__6FE99F9F");

                    b.HasOne("DataAccess.Entities.Card", "Card")
                        .WithMany("Transactions")
                        .HasForeignKey("CardId")
                        .HasConstraintName("FK__transacti__card___6EF57B66");

                    b.HasOne("DataAccess.Entities.Loan", "Loan")
                        .WithMany("Transactions")
                        .HasForeignKey("LoanId")
                        .HasConstraintName("FK__transacti__loan___70DDC3D8");

                    b.Navigation("Account");

                    b.Navigation("Card");

                    b.Navigation("Loan");
                });

            modelBuilder.Entity("DataAccess.Entities.Account", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("DataAccess.Entities.Business", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Cards");

                    b.Navigation("Loans");
                });

            modelBuilder.Entity("DataAccess.Entities.Card", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("DataAccess.Entities.Loan", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("DataAccess.Entities.User", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Cards");
                });
#pragma warning restore 612, 618
        }
    }
}