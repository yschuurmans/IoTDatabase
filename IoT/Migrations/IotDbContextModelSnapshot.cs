﻿// <auto-generated />
using System;
using IoT;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IoT.Migrations
{
    [DbContext(typeof(IotDbContext))]
    partial class IotDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("IoT.Item", b =>
                {
                    b.Property<string>("Key")
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.Property<int>("CollectionFK")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("LastUpdaterFK")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.HasKey("Key", "CollectionFK");

                    b.HasIndex("CollectionFK");

                    b.HasIndex("LastUpdaterFK");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("IoT.Models.ApiKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("CanWrite")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Key")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("ApiKey");
                });

            modelBuilder.Entity("IoT.Models.Collection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Collection");
                });

            modelBuilder.Entity("IoT.Item", b =>
                {
                    b.HasOne("IoT.Models.Collection", "Collection")
                        .WithMany("Items")
                        .HasForeignKey("CollectionFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Collection_Item");

                    b.HasOne("IoT.Models.ApiKey", "LastUpdater")
                        .WithMany("EditedItems")
                        .HasForeignKey("LastUpdaterFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ApiKey_Item");

                    b.Navigation("Collection");

                    b.Navigation("LastUpdater");
                });

            modelBuilder.Entity("IoT.Models.ApiKey", b =>
                {
                    b.Navigation("EditedItems");
                });

            modelBuilder.Entity("IoT.Models.Collection", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
