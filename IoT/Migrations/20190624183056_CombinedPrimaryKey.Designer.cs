﻿// <auto-generated />
using IoT;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IoT.Migrations
{
    [DbContext(typeof(IotDbContext))]
    [Migration("20190624183056_CombinedPrimaryKey")]
    partial class CombinedPrimaryKey
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("IoT.Item", b =>
                {
                    b.Property<string>("Key");

                    b.Property<int>("CollectionFK");

                    b.Property<int>("Id");

                    b.Property<bool>("IsPublic");

                    b.Property<string>("Value");

                    b.HasKey("Key", "CollectionFK");

                    b.HasIndex("CollectionFK");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("IoT.Models.ApiKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("CanWrite");

                    b.Property<string>("Key");

                    b.HasKey("Id");

                    b.ToTable("ApiKey");
                });

            modelBuilder.Entity("IoT.Models.Collection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.HasKey("Id");

                    b.ToTable("Collection");
                });

            modelBuilder.Entity("IoT.Item", b =>
                {
                    b.HasOne("IoT.Models.Collection", "Collection")
                        .WithMany("Items")
                        .HasForeignKey("CollectionFK")
                        .HasConstraintName("FK_Article_Source")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
