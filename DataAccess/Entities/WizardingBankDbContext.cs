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
            entity.HasKey(e => e.Id).HasName("PK__accounts__3213E83FDE898024");

            entity.ToTable("accounts");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("account_number");
            entity.Property(e => e.Balance)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("balance");
            entity.Property(e => e.BusinessId).HasColumnName("business_id");
            entity.Property(e => e.RoutingNumber)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("routing_number");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            // entity.HasOne(d => d.Business).WithMany(p => p.Accounts)
            //     .HasForeignKey(d => d.BusinessId)
            //     .HasConstraintName("FK__accounts__busine__656C112C");

            // entity.HasOne(d => d.User).WithMany(p => p.Accounts)
            //     .HasForeignKey(d => d.UserId)
            //     .HasConstraintName("FK__accounts__user_i__6477ECF3");
        });

        modelBuilder.Entity<Business>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__business__3213E83FD089B494");

            entity.ToTable("business");

            entity.HasIndex(e => e.Email, "UQ__business__AB6E616497C24BE0").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__business__F3DBC57216CEA674").IsUnique();

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
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("wallet");
        });

        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__cards__3213E83F8065BDE1");

            entity.ToTable("cards");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Balance)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("balance");
            entity.Property(e => e.BusinessId).HasColumnName("business_id");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("card_number");
            entity.Property(e => e.Cvv)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("cvv");
            entity.Property(e => e.ExpiryDate)
                .HasColumnType("datetime")
                .HasColumnName("expiry_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__loans__3213E83F8C2F8E66");

            entity.ToTable("loans");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("amount");
            entity.Property(e => e.BusinessId).HasColumnName("business_id");
            entity.Property(e => e.DateLoaned)
                .HasColumnType("datetime")
                .HasColumnName("date_loaned");
            entity.Property(e => e.InterestRate)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("interest_rate");
            entity.Property(e => e.LoanPaid)
                .HasColumnType("datetime")
                .HasColumnName("loan_paid");

        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__transact__3213E83FC82F76D6");

            entity.ToTable("transactions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("amount");
            entity.Property(e => e.CardId).HasColumnName("card_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.LoanId).HasColumnName("loan_id");

            // entity.HasOne(d => d.Account).WithMany(p => p.Transactions)
            //     .HasForeignKey(d => d.AccountId)
            //     .HasConstraintName("FK__transacti__accou__6FE99F9F");

            // entity.HasOne(d => d.Card).WithMany(p => p.Transactions)
            //     .HasForeignKey(d => d.CardId)
            //     .HasConstraintName("FK__transacti__card___6EF57B66");

            // entity.HasOne(d => d.Loan).WithMany(p => p.Transactions)
            //     .HasForeignKey(d => d.LoanId)
            //     .HasConstraintName("FK__transacti__loan___70DDC3D8");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F555EE9F3");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E6164E25936DC").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__users__F3DBC57215D5AFFC").IsUnique();

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
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("wallet");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
