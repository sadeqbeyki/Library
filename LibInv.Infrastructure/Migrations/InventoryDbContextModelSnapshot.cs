﻿// <auto-generated />
using System;
using LibInventory.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LibInventory.Infrastructure.Migrations
{
    [DbContext(typeof(InventoryDbContext))]
    partial class InventoryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LibInventory.Domain.InventoryAgg.Inventory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("InStock")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLoaned")
                        .HasColumnType("bit");

                    b.Property<double>("UnitPrice")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Inventory", (string)null);
                });

            modelBuilder.Entity("LibInventory.Domain.InventoryAgg.Inventory", b =>
                {
                    b.OwnsMany("LibInventory.Domain.InventoryAgg.InventoryOperation", "Operations", b1 =>
                        {
                            b1.Property<long>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("bigint");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<long>("Id"));

                            b1.Property<long>("Count")
                                .HasColumnType("bigint");

                            b1.Property<long>("CurrentCount")
                                .HasColumnType("bigint");

                            b1.Property<string>("Description")
                                .IsRequired()
                                .HasMaxLength(1000)
                                .HasColumnType("nvarchar(1000)");

                            b1.Property<int>("InventoryId")
                                .HasColumnType("int");

                            b1.Property<long>("LendId")
                                .HasColumnType("bigint");

                            b1.Property<bool>("Operation")
                                .HasColumnType("bit");

                            b1.Property<DateTime>("OperationDate")
                                .HasColumnType("datetime2");

                            b1.Property<Guid>("OperatorId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("Id");

                            b1.HasIndex("InventoryId");

                            b1.ToTable("InventoryOperations", (string)null);

                            b1.WithOwner("Inventory")
                                .HasForeignKey("InventoryId");

                            b1.Navigation("Inventory");
                        });

                    b.Navigation("Operations");
                });
#pragma warning restore 612, 618
        }
    }
}
