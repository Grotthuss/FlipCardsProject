using FlipCardProject.Enums;
using FlipCardProject.Models;
using FlipCardProject.Records;
using Microsoft.EntityFrameworkCore;

namespace FlipCardProject.Data;

public class DataContext : DbContext
{
    public DbSet<FlipcardSet> FlipCardSets { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<FlipcardSet>(entity =>
        {
            entity.Property("Id").ValueGeneratedOnAdd().IsRequired();
            entity.HasKey(p => p.Id);
            entity.Property(p => p.SetName).IsRequired();
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
        });
       
        
    }
    
    
}