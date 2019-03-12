﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Snow.AuthorityManagement.Data;

namespace Snow.AuthorityManagement.Data.Migrations
{
    [DbContext(typeof(AuthorityManagementContext))]
    [Migration("20190312062315_adduserAndRoleAndPermissionRelation")]
    partial class adduserAndRoleAndPermissionRelation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Snow.AuthorityManagement.Core.Entities.Authorization.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationTime");

                    b.Property<bool>("IsGranted");

                    b.Property<string>("Name");

                    b.Property<int?>("RoleID");

                    b.Property<int?>("UserID");

                    b.HasKey("Id");

                    b.HasIndex("RoleID");

                    b.HasIndex("UserID");

                    b.ToTable("Permission");
                });

            modelBuilder.Entity("Snow.AuthorityManagement.Core.Entities.Authorization.Role", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("AddTime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("Sort");

                    b.HasKey("ID");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Snow.AuthorityManagement.Core.Entities.Authorization.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("AddTime");

                    b.Property<bool>("CanUse");

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(11);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Snow.AuthorityManagement.Core.Entities.Authorization.Permission", b =>
                {
                    b.HasOne("Snow.AuthorityManagement.Core.Entities.Authorization.Role", "Role")
                        .WithMany("Permissions")
                        .HasForeignKey("RoleID");

                    b.HasOne("Snow.AuthorityManagement.Core.Entities.Authorization.User", "User")
                        .WithMany("Permissions")
                        .HasForeignKey("UserID");
                });
#pragma warning restore 612, 618
        }
    }
}