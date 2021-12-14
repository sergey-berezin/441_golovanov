﻿// <auto-generated />
using System;
using ConsoleApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ConsoleApp.Migrations
{
    [DbContext(typeof(ImageDB))]
    [Migration("20211214200643_Box")]
    partial class Box
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.22");

            modelBuilder.Entity("ConsoleApp.Box", b =>
                {
                    b.Property<int>("BoxId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<float>("Confidence")
                        .HasColumnType("REAL");

                    b.Property<int>("ImageId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Label")
                        .HasColumnType("TEXT");

                    b.Property<int>("x1")
                        .HasColumnType("INTEGER");

                    b.Property<int>("x2")
                        .HasColumnType("INTEGER");

                    b.Property<int>("x3")
                        .HasColumnType("INTEGER");

                    b.Property<int>("x4")
                        .HasColumnType("INTEGER");

                    b.HasKey("BoxId");

                    b.HasIndex("ImageId");

                    b.ToTable("boxes");
                });

            modelBuilder.Entity("ConsoleApp.Image", b =>
                {
                    b.Property<int>("ImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BLOBImageBlobId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ImageHash")
                        .HasColumnType("INTEGER");

                    b.HasKey("ImageId");

                    b.HasIndex("BLOBImageBlobId");

                    b.ToTable("images");
                });

            modelBuilder.Entity("ConsoleApp.ImageBlob", b =>
                {
                    b.Property<int>("ImageBlobId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Img")
                        .HasColumnType("BLOB");

                    b.HasKey("ImageBlobId");

                    b.ToTable("ImageBlob");
                });

            modelBuilder.Entity("ConsoleApp.Box", b =>
                {
                    b.HasOne("ConsoleApp.Image", "Image")
                        .WithMany("boxes")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ConsoleApp.Image", b =>
                {
                    b.HasOne("ConsoleApp.ImageBlob", "BLOB")
                        .WithMany()
                        .HasForeignKey("BLOBImageBlobId");
                });
#pragma warning restore 612, 618
        }
    }
}
