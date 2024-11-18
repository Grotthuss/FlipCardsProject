using FlipCardProject.Enums;
using FlipCardProject.Models;
using FlipCardProject.Records;
using Microsoft.EntityFrameworkCore;

namespace FlipCardProject.Data;

public class DataContext : DbContext
{
    public DbSet<FlipcardSet> FlipCardSets { get; set; }
    public DbSet<User> Users { get; set; }
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
            entity.OwnsMany(u => u.FlipcardSets, s =>
            {
                var t = s.WithOwner().HasForeignKey("UserId");
                s.Property(p => p.Id).ValueGeneratedOnAdd();
                s.HasKey(f => new { f.UserId,f.Id });
                s.Property(p => p.Name).IsRequired().HasMaxLength(100);

                s.OwnsMany(f => f.FlipcardsList, a =>
                {
                    a.WithOwner().HasForeignKey("UserId", "SetId"); // Foreign key in Flipcard
                    a.Property(f => f.Id).ValueGeneratedOnAdd().IsRequired();
                    a.HasKey(f => f.Id);
                    a.Property(f => f.Concept).HasMaxLength(300).IsRequired();
                    a.Property(f => f.Question).HasMaxLength(300).IsRequired();
                    a.Property(f => f.Mnemonic).HasMaxLength(300).IsRequired();
                    a.Property(f => f.State)
                        .IsRequired()
                        .HasConversion(
                            v => v._state.ToString(), // Converts enum to string
                            v => new FlipcardState { _state = (FlipCardStateEnum)Enum.Parse(typeof(FlipCardStateEnum), v) }
                        );
                });

            });
        });
        
        
        /*modelBuilder.Entity<FlipcardSet>(entity =>
        {
            entity.Property("Id").ValueGeneratedOnAdd().IsRequired();
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired();
            entity.OwnsMany(p => p.FlipcardsList,a =>
            {
                a.WithOwner().HasForeignKey("OwnerId"); // Foreign key in Flipcard
                a.Property("Id").ValueGeneratedOnAdd().IsRequired();
                a.HasKey(f => f.Id);
                a.Property(f => f.Concept).HasMaxLength(100).IsRequired();
                a.Property(f => f.Question).HasMaxLength(100).IsRequired();
                a.Property(f => f.Mnemonic).HasMaxLength(100).IsRequired();
                a.Property(f => f.State)
                    .IsRequired()
                    .HasConversion(
                        v => v._state.ToString(), // Converts enum to string
                        v => new FlipcardState { _state = (FlipCardStateEnum)Enum.Parse(typeof(FlipCardStateEnum), v) }
                    );
            });
        });*/
       
        /*modelBuilder.Entity<FlipcardSet>(entity =>
        {
            entity.Property(f => f.Id).ValueGeneratedOnAdd().IsRequired();
            entity.HasKey(f => new { f.Id, f.UserId }); // Composite key

            entity.Property(f => f.Name).IsRequired();

            entity.HasOne(f => f.User)
                .WithMany(u => u.FlipcardSets)
                .HasForeignKey(f => f.UserId);

            entity.OwnsMany(f => f.FlipcardsList, a =>
            {
                a.WithOwner().HasForeignKey(f => new { f.FlipcardSetId, f.UserId }); // Foreign key in Flipcard
                a.Property(f => f.Id).ValueGeneratedOnAdd().IsRequired();
                a.HasKey(f => f.Id);
                a.Property(f => f.Concept).HasMaxLength(300).IsRequired();
                a.Property(f => f.Question).HasMaxLength(300).IsRequired();
                a.Property(f => f.Mnemonic).HasMaxLength(300).IsRequired();
                a.Property(f => f.State)
                    .IsRequired()
                    .HasConversion(
                        v => v.ToString(), // Converts enum to string
                        v => (FlipcardState)Enum.Parse(typeof(FlipcardState), v)
                    );
            });
        });*/
        
    }
    
    
}