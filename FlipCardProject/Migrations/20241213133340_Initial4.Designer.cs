﻿// <auto-generated />
using FlipCardProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FlipCardProject.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20241213133340_Initial4")]
    partial class Initial4
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FlipCardProject.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FlipCardProject.Models.User", b =>
                {
                    b.OwnsMany("FlipCardProject.Models.FlipcardSet", "FlipcardSets", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<int>("UserId")
                                .HasColumnType("int");

                            b1.HasKey("Id");

                            b1.HasIndex("UserId");

                            b1.ToTable("FlipCardSets");

                            b1.WithOwner()
                                .HasForeignKey("UserId");

                            b1.OwnsMany("FlipCardProject.Models.Flipcard", "FlipcardsList", b2 =>
                                {
                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("int");

                                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b2.Property<int>("Id"));

                                    b2.Property<string>("Concept")
                                        .IsRequired()
                                        .HasMaxLength(300)
                                        .HasColumnType("nvarchar(300)");

                                    b2.Property<string>("Mnemonic")
                                        .IsRequired()
                                        .HasMaxLength(300)
                                        .HasColumnType("nvarchar(300)");

                                    b2.Property<string>("Question")
                                        .IsRequired()
                                        .HasMaxLength(300)
                                        .HasColumnType("nvarchar(300)");

                                    b2.Property<int>("SetId")
                                        .HasColumnType("int");

                                    b2.HasKey("Id");

                                    b2.HasIndex("SetId");

                                    b2.ToTable("Flipcards");

                                    b2.WithOwner()
                                        .HasForeignKey("SetId");
                                });

                            b1.Navigation("FlipcardsList");
                        });

                    b.Navigation("FlipcardSets");
                });
#pragma warning restore 612, 618
        }
    }
}
