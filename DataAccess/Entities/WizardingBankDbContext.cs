using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities;

public partial class WizardingBankDbContext : DbContext
{
    public WizardingBankDbContext(DbContextOptions<WizardingBankDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Business> Businesses { get; set; }

    public virtual DbSet<Card> Cards { get; set; }

    public virtual DbSet<Loan> Loans { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__accounts__3213E83F4B9519F7");

            entity.ToTable("accounts");

            entity.HasIndex(e => e.AccountNumber, "UQ__accounts__AF91A6AD66153070").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("account_number");
            entity.Property(e => e.Balance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("balance");
            entity.Property(e => e.BusinessId).HasColumnName("business_id");
            entity.Property(e => e.RoutingNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("routing_number");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Business).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.BusinessId)
                .HasConstraintName("FK__accounts__busine__1CBC4616");

            entity.HasOne(d => d.User).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__accounts__user_i__1BC821DD");
        });

        modelBuilder.Entity<Business>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__business__3213E83FD089B494");

            entity.ToTable("business");

            entity.HasIndex(e => e.Email, "UQ__business__AB6E616497C24BE0")
                .IsUnique()
                .HasFilter("([email] IS NOT NULL)");

            entity.HasIndex(e => e.Username, "UQ__business__F3DBC57216CEA674")
                .IsUnique()
                .HasFilter("([username] IS NOT NULL)");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.Bin)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("bin");
            entity.Property(e => e.BusinessName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("business_name");
            entity.Property(e => e.BusinessType)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("business_type");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("username");
            entity.Property(e => e.Wallet)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("wallet");
        });

        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__cards__3213E83F8065BDE1");

            entity.ToTable("cards");

            entity.HasIndex(e => e.BusinessId, "IX_cards_business_id");

            entity.HasIndex(e => e.UserId, "IX_cards_user_id");

            entity.HasIndex(e => e.CardNumber, "UQ__cards__1E6E0AF423FEDC1D").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Balance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("balance");
            entity.Property(e => e.BusinessId).HasColumnName("business_id");
            entity.Property(e => e.CardNumber).HasColumnName("card_number");
            entity.Property(e => e.Cvv).HasColumnName("cvv");
            entity.Property(e => e.ExpiryDate)
                .HasColumnType("datetime")
                .HasColumnName("expiry_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Business).WithMany(p => p.Cards)
                .HasForeignKey(d => d.BusinessId)
                .HasConstraintName("FK__cards__business___693CA210");

            entity.HasOne(d => d.User).WithMany(p => p.Cards)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__cards__user_id__68487DD7");
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__loans__3213E83F8C2F8E66");

            entity.ToTable("loans");

            entity.HasIndex(e => e.BusinessId, "IX_loans_business_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.BusinessId).HasColumnName("business_id");
            entity.Property(e => e.DateLoaned)
                .HasColumnType("datetime")
                .HasColumnName("date_loaned");
            entity.Property(e => e.InterestRate)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("interest_rate");
            entity.Property(e => e.LoanPaid)
                .HasColumnType("datetime")
                .HasColumnName("loan_paid");

            entity.HasOne(d => d.Business).WithMany(p => p.Loans)
                .HasForeignKey(d => d.BusinessId)
                .HasConstraintName("FK__loans__business___6C190EBB");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity
                .HasKey(e => e.Id).HasName("PK__transact__3213E8382286");


            entity.ToTable("transactions");

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CardId).HasColumnName("card_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.RecipientId).HasColumnName("recipient_id");
            entity.Property(e => e.SenderId).HasColumnName("sender_id");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F555EE9F3");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E6164E25936DC").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__users__F3DBC57215D5AFFC")
                .IsUnique()
                .HasFilter("([username] IS NOT NULL)");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("full_name");
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("username");
            entity.Property(e => e.Wallet)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("wallet");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
