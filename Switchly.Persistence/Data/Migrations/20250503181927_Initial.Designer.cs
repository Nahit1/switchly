﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Switchly.Persistence.Db;

#nullable disable

namespace Switchly.Persistence.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250503181927_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Switchly.Domain.Entities.FeatureFlag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("FeatureFlags");
                });

            modelBuilder.Entity("Switchly.Domain.Entities.FeatureFlagEnvironment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Environment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("FeatureFlagId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean");

                    b.Property<int>("RolloutPercentage")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FeatureFlagId");

                    b.ToTable("FeatureFlagEnvironments");
                });

            modelBuilder.Entity("Switchly.Domain.Entities.Organization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("Switchly.Domain.Entities.SegmentRule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("FeatureFlagId")
                        .HasColumnType("uuid");

                    b.Property<string>("Operator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<string>("Property")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RolloutPercentage")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FeatureFlagId");

                    b.ToTable("SegmentRules");
                });

            modelBuilder.Entity("Switchly.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uuid");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Switchly.Domain.Entities.FeatureFlag", b =>
                {
                    b.HasOne("Switchly.Domain.Entities.Organization", "Organization")
                        .WithMany("FeatureFlags")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("Switchly.Domain.Entities.FeatureFlagEnvironment", b =>
                {
                    b.HasOne("Switchly.Domain.Entities.FeatureFlag", "FeatureFlag")
                        .WithMany("Environments")
                        .HasForeignKey("FeatureFlagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FeatureFlag");
                });

            modelBuilder.Entity("Switchly.Domain.Entities.SegmentRule", b =>
                {
                    b.HasOne("Switchly.Domain.Entities.FeatureFlag", "FeatureFlag")
                        .WithMany("SegmentRules")
                        .HasForeignKey("FeatureFlagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FeatureFlag");
                });

            modelBuilder.Entity("Switchly.Domain.Entities.User", b =>
                {
                    b.HasOne("Switchly.Domain.Entities.Organization", "Organization")
                        .WithMany("Users")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("Switchly.Domain.Entities.FeatureFlag", b =>
                {
                    b.Navigation("Environments");

                    b.Navigation("SegmentRules");
                });

            modelBuilder.Entity("Switchly.Domain.Entities.Organization", b =>
                {
                    b.Navigation("FeatureFlags");

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
