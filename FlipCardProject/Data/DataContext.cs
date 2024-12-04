using FlipCardProject.Enums;
using FlipCardProject.Models;
using FlipCardProject.Records;
using Microsoft.EntityFrameworkCore;

namespace FlipCardProject.Data;

public class DataContext : DbContext
{
    public DbSet<FlipcardSet> FlipCardSets { get; set; }
    public DbSet<User> Users { get; set; }
    
    //public DbSet<User> Flipcards { get; set; }
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Id).ValueGeneratedOnAdd().IsRequired();
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Password).IsRequired().HasMaxLength(100);
            entity.OwnsMany(u => u.FlipcardSets, s =>
            {
                s.Property(p => p.Id).ValueGeneratedOnAdd();
                s.HasKey(p => p.Id);
                s.WithOwner().HasForeignKey("UserId");
                s.Property(p => p.Name).IsRequired().HasMaxLength(100);

                s.OwnsMany(f => f.FlipcardsList, a =>
                {
                    a.WithOwner().HasForeignKey("SetId"); // Foreign key in Flipcard
                    a.Property(f => f.Id).ValueGeneratedOnAdd().IsRequired();
                    a.HasKey(f => f.Id);
                    a.Property(f => f.Concept).HasMaxLength(300).IsRequired();
                    a.Property(f => f.Question).HasMaxLength(300).IsRequired();
                    a.Property(f => f.Mnemonic).HasMaxLength(300).IsRequired();
                    
                    /*a.Property(f => f.State)
                        .IsRequired()
                        .HasConversion(
                            v => v._state.ToString(), // Converts enum to string
                            v => new FlipcardState { _state = (FlipCardStateEnum)Enum.Parse(typeof(FlipCardStateEnum), v) }
                        );*/
                });

            });
        });
        
        /*modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Id).ValueGeneratedOnAdd().IsRequired();
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.HasMany<FlipcardSet>()
                .WithOne()
                .HasForeignKey(f => f.UserId)
                .IsRequired();
            

        });

        modelBuilder.Entity<FlipcardSet>(entity =>
        {
            entity.Property(s => s.Id).ValueGeneratedOnAdd().IsRequired();
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
            
            // Relationship to Flipcards
            entity.HasMany<Flipcard>()
                .WithOne()
                .HasForeignKey("SetId")
                .IsRequired();
            entity.HasOne<User>()
                .WithMany(u => u.FlipcardSets) // Navigation property in User
                .HasForeignKey("UserId")      // Single foreign key in the database
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        
        
        modelBuilder.Entity<Flipcard>(entity =>
        {
            entity.Property(f => f.Id).ValueGeneratedOnAdd().IsRequired();
            entity.HasKey(f => f.Id);
            entity.Property(f => f.Concept).HasMaxLength(30).IsRequired();
            entity.Property(f => f.Question).HasMaxLength(30).IsRequired();
            entity.Property(f => f.Mnemonic).HasMaxLength(30).IsRequired();
            entity.Property(f => f.State)
                .IsRequired()
                .HasConversion(
                    v => v._state.ToString(),
                    v => new FlipcardState { _state = (FlipCardStateEnum)Enum.Parse(typeof(FlipCardStateEnum), v) }
                );
            entity.HasOne<FlipcardSet>()
                .WithMany(f => f.FlipcardsList) // Navigation property in FlipcardSet
                .HasForeignKey("SetId")        // Single foreign key in the database
                .OnDelete(DeleteBehavior.Cascade);
        });*/
    }
    
    
}