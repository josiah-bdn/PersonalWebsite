﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Persistence;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20231212202303_modifiedPasswordResetTable")]
    partial class modifiedPasswordResetTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Data.Entities.AppUser", b =>
                {
                    b.Property<Guid>("AppUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AppUserId");

                    b.ToTable("AppUser");
                });

            modelBuilder.Entity("Data.Entities.Authentication", b =>
                {
                    b.Property<Guid>("AppUserId")
                        .HasColumnType("uuid");

                    b.Property<string>("HashPassword")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("LoginCount")
                        .HasColumnType("integer");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AppUserId");

                    b.ToTable("Authentication");
                });

            modelBuilder.Entity("Data.Entities.PasswordResetRequest", b =>
                {
                    b.Property<Guid>("PasswordRequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AppUserId")
                        .HasColumnType("uuid");

                    b.Property<int>("Code")
                        .HasColumnType("integer");

                    b.Property<int>("CodeEntryCount")
                        .HasColumnType("integer");

                    b.Property<bool>("IsExpiredOrFailed")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("SendDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("UserHasValidated")
                        .HasColumnType("boolean");

                    b.HasKey("PasswordRequestId");

                    b.HasIndex("AppUserId");

                    b.ToTable("PasswordResetRequests");
                });

            modelBuilder.Entity("Data.Entities.Authentication", b =>
                {
                    b.HasOne("Data.Entities.AppUser", "AppUser")
                        .WithOne("Authentication")
                        .HasForeignKey("Data.Entities.Authentication", "AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("Data.Entities.PasswordResetRequest", b =>
                {
                    b.HasOne("Data.Entities.AppUser", "AppUser")
                        .WithMany("PasswordResetRequests")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("Data.Entities.AppUser", b =>
                {
                    b.Navigation("Authentication")
                        .IsRequired();

                    b.Navigation("PasswordResetRequests");
                });
#pragma warning restore 612, 618
        }
    }
}