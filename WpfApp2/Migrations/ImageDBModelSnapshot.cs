﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WpfApp2;

namespace WpfApp2.Migrations
{
    [DbContext(typeof(ImageDB))]
    partial class ImageDBModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.22");

            modelBuilder.Entity("WpfApp2.Box", b =>
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

                    b.Property<float>("x1")
                        .HasColumnType("REAL");

                    b.Property<float>("x2")
                        .HasColumnType("REAL");

                    b.Property<float>("x3")
                        .HasColumnType("REAL");

                    b.Property<float>("x4")
                        .HasColumnType("REAL");

                    b.HasKey("BoxId");

                    b.HasIndex("ImageId");

                    b.ToTable("boxes");
                });

            modelBuilder.Entity("WpfApp2.Image", b =>
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

            modelBuilder.Entity("WpfApp2.ImageBlob", b =>
                {
                    b.Property<int>("ImageBlobId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Img")
                        .HasColumnType("BLOB");

                    b.HasKey("ImageBlobId");

                    b.ToTable("ImageBlob");
                });

            modelBuilder.Entity("WpfApp2.Box", b =>
                {
                    b.HasOne("WpfApp2.Image", "Image")
                        .WithMany("boxes")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WpfApp2.Image", b =>
                {
                    b.HasOne("WpfApp2.ImageBlob", "BLOB")
                        .WithMany()
                        .HasForeignKey("BLOBImageBlobId");
                });
#pragma warning restore 612, 618
        }
    }
}
