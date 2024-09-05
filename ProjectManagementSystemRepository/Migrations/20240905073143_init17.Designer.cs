﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectManagementSystemRepository;

#nullable disable

namespace ProjectManagementSystemRepository.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240905073143_init17")]
    partial class init17
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProjectManagementSystemCore.Models.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FileUploadId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ManagerId")
                        .HasColumnType("int");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<Guid>("UserIdentityId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("FileUploadId");

                    b.HasIndex("ManagerId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("UserId");

                    b.HasIndex("UserIdentityId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("ProjectManagementSystemCore.Models.FileUpload", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UserIdentityId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserIdentityId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("ProjectManagementSystemCore.Models.Job", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("ProjectManagementSystemCore.Models.Manager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<Guid>("UserIdentityId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserIdentityId");

                    b.ToTable("Managers");
                });

            modelBuilder.Entity("ProjectManagementSystemCore.Models.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("FileUploadId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FileUploadId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("ProjectManagementSystemCore.Models.ProjectManager", b =>
                {
                    b.Property<int>("ManagerId")
                        .HasColumnType("int");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ManagerId", "ProjectId");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectManager");
                });

            modelBuilder.Entity("ProjectManagementSystemCore.Models.ProjectUser", b =>
                {
                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ProjectId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("ProjectUser");
                });

            modelBuilder.Entity("ProjectManagementSystemCore.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ProjectManagementSystemCore.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<Guid>("UserIdentityId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserIdentityId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ProjectManagementSystemCore.Models.UserIdentity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserIdentities");
                });

            modelBuilder.Entity("ProjectManagementSystemCore.Models.Comment", b =>
                {
                    b.HasOne("ProjectManagementSystemCore.Models.FileUpload", "FileUpload")
                        .WithMany()
                        .HasForeignKey("FileUploadId");

                    b.HasOne("ProjectManagementSystemCore.Models.Manager", null)
                        .WithMany("Comments")
                        .HasForeignKey("ManagerId");

                    b.HasOne("ProjectManagementSystemCore.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectManagementSystemCore.Models.User", null)
                        .WithMany("Comments")
                        .HasForeignKey("UserId");

                    b.HasOne("ProjectManagementSystemCore.Models.UserIdentity", "UserIdentity")
                        .WithMany()
                        .HasForeignKey("UserIdentityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FileUpload");

                    b.Navigation("Project");

                    b.Navigation("UserIdentity");
                });

            modelBuilder.Entity("ProjectManagementSystemCore.Models.FileUpload", b =>
                {
                    b.HasOne("ProjectManagementSystemCore.Models.UserIdentity", "UserIdentity")
                        .WithMany()
                        .HasForeignKey("UserIdentityId");

                    b.Navigation("UserIdentity");
                });

            modelBuilder.Entity("ProjectManagementSystemCore.Models.Job", b =>
                {
                    b.HasOne("ProjectManagementSystemCore.Models.User", "User")
                        .WithMany("Jobs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectManagementSystemCore.Models.Manager", b =>
                {
                    b.HasOne("ProjectManagementSystemCore.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectManagementSystemCore.Models.UserIdentity", "UserIdentity")
                        .WithMany()
                        .HasForeignKey("UserIdentityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("UserIdentity");
                });

            modelBuilder.Entity("ProjectManagementSystemCore.Models.Project", b =>
                {
                    b.HasOne("ProjectManagementSystemCore.Models.FileUpload", "FileUpload")
                        .WithMany()
                        .HasForeignKey("FileUploadId");

                    b.Navigation("FileUpload");
                });

            modelBuilder.Entity("ProjectManagementSystemCore.Models.ProjectManager", b =>
                {
                    b.HasOne("ProjectManagementSystemCore.Models.Manager", null)
                        .WithMany("ProjectManagers")
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectManagementSystemCore.Models.Project", null)
                        .WithMany("ProjectManagers")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectManagementSystemCore.Models.ProjectUser", b =>
                {
                    b.HasOne("ProjectManagementSystemCore.Models.Project", null)
                        .WithMany("ProjectUsers")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectManagementSystemCore.Models.User", null)
                        .WithMany("ProjectUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectManagementSystemCore.Models.User", b =>
                {
                    b.HasOne("ProjectManagementSystemCore.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectManagementSystemCore.Models.UserIdentity", "UserIdentity")
                        .WithMany()
                        .HasForeignKey("UserIdentityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("UserIdentity");
                });

            modelBuilder.Entity("ProjectManagementSystemCore.Models.Manager", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("ProjectManagers");
                });

            modelBuilder.Entity("ProjectManagementSystemCore.Models.Project", b =>
                {
                    b.Navigation("ProjectManagers");

                    b.Navigation("ProjectUsers");
                });

            modelBuilder.Entity("ProjectManagementSystemCore.Models.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Jobs");

                    b.Navigation("ProjectUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
