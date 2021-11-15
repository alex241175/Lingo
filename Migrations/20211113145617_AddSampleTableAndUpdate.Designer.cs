﻿// <auto-generated />
using System;
using Lingo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Lingo.Migrations
{
    [DbContext(typeof(LingoDbContext))]
    [Migration("20211113145617_AddSampleTableAndUpdate")]
    partial class AddSampleTableAndUpdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.11");

            modelBuilder.Entity("Lingo.Models.Essay", b =>
                {
                    b.Property<int>("EssayId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("SampleId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Source")
                        .HasColumnType("TEXT");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("EssayId");

                    b.ToTable("Essays");
                });

            modelBuilder.Entity("Lingo.Models.Sample", b =>
                {
                    b.Property<int>("SampleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Source")
                        .HasColumnType("TEXT");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("SampleId");

                    b.ToTable("Samples");
                });

            modelBuilder.Entity("Lingo.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Lingo.Models.Vocab", b =>
                {
                    b.Property<int>("VocabId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .HasColumnType("TEXT");

                    b.Property<string>("Chinese")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("TEXT");

                    b.Property<int?>("EssayId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("Review")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("VocabId");

                    b.HasIndex("EssayId");

                    b.ToTable("Vocabs");
                });

            modelBuilder.Entity("Lingo.Models.Vocab", b =>
                {
                    b.HasOne("Lingo.Models.Essay", null)
                        .WithMany("Vocabs")
                        .HasForeignKey("EssayId");
                });

            modelBuilder.Entity("Lingo.Models.Essay", b =>
                {
                    b.Navigation("Vocabs");
                });
#pragma warning restore 612, 618
        }
    }
}
