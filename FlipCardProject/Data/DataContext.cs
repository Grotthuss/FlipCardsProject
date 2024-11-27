using FlipCardProject.Enums;
using FlipCardProject.Models;
using FlipCardProject.Records;
using Microsoft.EntityFrameworkCore;

namespace FlipCardProject.Data;

public class DataContext : DbContext
{
    public DbSet<FlipcardSet> FlipCardSets { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Usert> Userst { get; set; }
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
                s.Property(p => p.Id).ValueGeneratedOnAdd();
                s.HasKey(f => new { f.UserId,f.Id });
                s.Property(p => p.Name).IsRequired().HasMaxLength(100);

                s.OwnsMany(f => f.FlipcardsList, a =>
                {
                    a.WithOwner().HasForeignKey("UserId", "SetId");
                    a.Property(f => f.Id).ValueGeneratedOnAdd().IsRequired();
                    a.HasKey(f => f.Id);
                    a.Property(f => f.Concept).HasMaxLength(300).IsRequired();
                    a.Property(f => f.Question).HasMaxLength(300).IsRequired();
                    a.Property(f => f.Mnemonic).HasMaxLength(300).IsRequired();
                    a.Property(f => f.State)
                        .IsRequired()
                        .HasConversion(
                            v => v._state.ToString(),
                            v => new FlipcardState { _state = (FlipCardStateEnum)Enum.Parse(typeof(FlipCardStateEnum), v) }
                        );
                });

            });
        });
        modelBuilder.Entity<Usert>(entity =>
        {
            entity.Property("Id").ValueGeneratedOnAdd().IsRequired();
            entity.HasKey(p => p.Id);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
            entity.Property(u => u.whatevz).IsRequired().HasMaxLength(100);
            
        });
    }
}