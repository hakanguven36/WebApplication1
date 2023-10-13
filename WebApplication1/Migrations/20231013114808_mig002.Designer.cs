﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication1.Models;

namespace WebApplication1.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20231013114808_mig002")]
    partial class mig002
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.17");

            modelBuilder.Entity("WebApplication1.Models.Annotation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ProjectID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("color")
                        .HasColumnType("TEXT");

                    b.Property<string>("name")
                        .HasColumnType("TEXT");

                    b.Property<string>("textColor")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("ProjectID");

                    b.ToTable("Annotation");
                });

            modelBuilder.Entity("WebApplication1.Models.Label", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AnnotationID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PhotoID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("beginX")
                        .HasColumnType("INTEGER");

                    b.Property<int>("beginY")
                        .HasColumnType("INTEGER");

                    b.Property<int>("endX")
                        .HasColumnType("INTEGER");

                    b.Property<int>("endY")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("AnnotationID");

                    b.HasIndex("PhotoID");

                    b.HasIndex("UserID");

                    b.ToTable("Label");
                });

            modelBuilder.Entity("WebApplication1.Models.Photo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProjectID")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("completed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("contentType")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("date")
                        .HasColumnType("TEXT");

                    b.Property<string>("extention")
                        .HasColumnType("TEXT");

                    b.Property<string>("imageFormat")
                        .HasColumnType("TEXT");

                    b.Property<string>("orjname")
                        .HasColumnType("TEXT");

                    b.Property<double>("sizekb")
                        .HasColumnType("REAL");

                    b.Property<string>("sysname")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("ProjectID");

                    b.ToTable("Photo");
                });

            modelBuilder.Entity("WebApplication1.Models.Project", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("name")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("WebApplication1.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("admin")
                        .HasColumnType("INTEGER");

                    b.Property<int>("hatali")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("kilitli")
                        .HasColumnType("INTEGER");

                    b.Property<string>("passwordEnc")
                        .HasColumnType("TEXT");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("WebApplication1.Models.Annotation", b =>
                {
                    b.HasOne("WebApplication1.Models.Project", "Project")
                        .WithMany("annoList")
                        .HasForeignKey("ProjectID");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("WebApplication1.Models.Label", b =>
                {
                    b.HasOne("WebApplication1.Models.Annotation", "Annotation")
                        .WithMany()
                        .HasForeignKey("AnnotationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication1.Models.Photo", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication1.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Annotation");

                    b.Navigation("Photo");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebApplication1.Models.Photo", b =>
                {
                    b.HasOne("WebApplication1.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("WebApplication1.Models.Project", b =>
                {
                    b.Navigation("annoList");
                });
#pragma warning restore 612, 618
        }
    }
}
