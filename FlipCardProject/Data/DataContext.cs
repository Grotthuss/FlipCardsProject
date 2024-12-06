using FlipCardProject.Enums;
using FlipCardProject.Models;
using FlipCardProject.Records;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
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
                });

            });
        });
        
        
    }
    
    
}