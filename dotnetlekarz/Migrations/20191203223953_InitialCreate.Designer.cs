﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using dotnetlekarz.Services;

namespace dotnetlekarz.Migrations
{
    [DbContext(typeof(DocDbContext))]
    [Migration("20191203223953_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0");

            modelBuilder.Entity("dotnetlekarz.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<string>("Surname")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserRole")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("dotnetlekarz.Models.Visit", b =>
                {
                    b.Property<int>("VisitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("DoctorId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VisitorId")
                        .HasColumnType("INTEGER");

                    b.HasKey("VisitId");

                    b.HasIndex("VisitorId");

                    b.HasIndex("DoctorId", "DateTime")
                        .IsUnique();

                    b.ToTable("Visits");
                });

            modelBuilder.Entity("dotnetlekarz.Models.Visit", b =>
                {
                    b.HasOne("dotnetlekarz.Models.User", "Doctor")
                        .WithMany("ManagedVisits")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dotnetlekarz.Models.User", "Visitor")
                        .WithMany("AttendedVisits")
                        .HasForeignKey("VisitorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}