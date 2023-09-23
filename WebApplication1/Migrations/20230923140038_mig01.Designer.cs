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
    [Migration("20230923140038_mig01")]
    partial class mig01
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.17");

            modelBuilder.Entity("WebApplication1.Models.Etiket", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("HamResimID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("choice")
                        .HasColumnType("INTEGER");

                    b.Property<int>("cursorCol")
                        .HasColumnType("INTEGER");

                    b.Property<int>("cursorRow")
                        .HasColumnType("INTEGER");

                    b.Property<int>("cursorSize")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("HamResimID");

                    b.ToTable("Etiket");
                });

            modelBuilder.Entity("WebApplication1.Models.HamResim", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProjeID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("UserID")
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

                    b.Property<int>("seenOrWhat")
                        .HasColumnType("INTEGER");

                    b.Property<double>("sizekb")
                        .HasColumnType("REAL");

                    b.Property<string>("sysname")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("ProjeID");

                    b.HasIndex("UserID");

                    b.ToTable("HamResim");
                });

            modelBuilder.Entity("WebApplication1.Models.Proje", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("name")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Proje");
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

            modelBuilder.Entity("WebApplication1.Models.UserSetting", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("canvasSize")
                        .HasColumnType("INTEGER");

                    b.Property<int>("seenOrWhat")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ucanCanvasSize")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("UserSetting");
                });

            modelBuilder.Entity("WebApplication1.Models.Etiket", b =>
                {
                    b.HasOne("WebApplication1.Models.HamResim", "HamResim")
                        .WithMany()
                        .HasForeignKey("HamResimID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HamResim");
                });

            modelBuilder.Entity("WebApplication1.Models.HamResim", b =>
                {
                    b.HasOne("WebApplication1.Models.Proje", "Proje")
                        .WithMany()
                        .HasForeignKey("ProjeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication1.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID");

                    b.Navigation("Proje");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebApplication1.Models.UserSetting", b =>
                {
                    b.HasOne("WebApplication1.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
