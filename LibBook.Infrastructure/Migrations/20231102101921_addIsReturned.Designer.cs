﻿// <auto-generated />
using System;
using LibBook.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LibBook.Infrastructure.Migrations
{
    [DbContext(typeof(BookDbContext))]
    [Migration("20231102101921_addIsReturned")]
    partial class addIsReturned
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LibBook.Domain.AuthorAgg.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(750)
                        .HasColumnType("nvarchar(750)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.ToTable("Authors", (string)null);
                });

            modelBuilder.Entity("LibBook.Domain.BookAgg.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(750)
                        .HasColumnType("nvarchar(750)");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Books", (string)null);
                });

            modelBuilder.Entity("LibBook.Domain.BookAuthor", b =>
                {
                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int>("AuthorBookId")
                        .HasColumnType("int");

                    b.HasKey("AuthorId", "AuthorBookId");

                    b.HasIndex("AuthorBookId");

                    b.ToTable("BookAuthors", (string)null);
                });

            modelBuilder.Entity("LibBook.Domain.BookCategoryAgg.BookCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(780)
                        .HasColumnType("nvarchar(780)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BookCategories", (string)null);
                });

            modelBuilder.Entity("LibBook.Domain.BookPublisher", b =>
                {
                    b.Property<int>("PublisherId")
                        .HasColumnType("int");

                    b.Property<int>("PublisherBookId")
                        .HasColumnType("int");

                    b.HasKey("PublisherId", "PublisherBookId");

                    b.HasIndex("PublisherBookId");

                    b.ToTable("BookPublishers", (string)null);
                });

            modelBuilder.Entity("LibBook.Domain.BookTranslator", b =>
                {
                    b.Property<int>("TranslatorId")
                        .HasColumnType("int");

                    b.Property<int>("TranslatorBookId")
                        .HasColumnType("int");

                    b.HasKey("TranslatorId", "TranslatorBookId");

                    b.HasIndex("TranslatorBookId");

                    b.ToTable("BookTranslators", (string)null);
                });

            modelBuilder.Entity("LibBook.Domain.BorrowAgg.Borrow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmployeeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("IdealReturnDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsReturned")
                        .HasColumnType("bit");

                    b.Property<string>("MemberID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReturnEmployeeID")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Borrows", (string)null);
                });

            modelBuilder.Entity("LibBook.Domain.PublisherAgg.Publisher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(750)
                        .HasColumnType("nvarchar(750)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.ToTable("Publishers", (string)null);
                });

            modelBuilder.Entity("LibBook.Domain.TranslatorAgg.Translator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(750)
                        .HasColumnType("nvarchar(750)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.ToTable("Translators", (string)null);
                });

            modelBuilder.Entity("LibBook.Domain.BookAgg.Book", b =>
                {
                    b.HasOne("LibBook.Domain.BookCategoryAgg.BookCategory", "Category")
                        .WithMany("Books")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("LibBook.Domain.BookAuthor", b =>
                {
                    b.HasOne("LibBook.Domain.BookAgg.Book", "Book")
                        .WithMany("BookAuthors")
                        .HasForeignKey("AuthorBookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibBook.Domain.AuthorAgg.Author", "Author")
                        .WithMany("AuthorBooks")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Book");
                });

            modelBuilder.Entity("LibBook.Domain.BookPublisher", b =>
                {
                    b.HasOne("LibBook.Domain.BookAgg.Book", "Book")
                        .WithMany("BookPublishers")
                        .HasForeignKey("PublisherBookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibBook.Domain.PublisherAgg.Publisher", "Publisher")
                        .WithMany("PublisherBooks")
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Publisher");
                });

            modelBuilder.Entity("LibBook.Domain.BookTranslator", b =>
                {
                    b.HasOne("LibBook.Domain.BookAgg.Book", "Book")
                        .WithMany("BookTranslators")
                        .HasForeignKey("TranslatorBookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibBook.Domain.TranslatorAgg.Translator", "Translator")
                        .WithMany("TranslatorBooks")
                        .HasForeignKey("TranslatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Translator");
                });

            modelBuilder.Entity("LibBook.Domain.AuthorAgg.Author", b =>
                {
                    b.Navigation("AuthorBooks");
                });

            modelBuilder.Entity("LibBook.Domain.BookAgg.Book", b =>
                {
                    b.Navigation("BookAuthors");

                    b.Navigation("BookPublishers");

                    b.Navigation("BookTranslators");
                });

            modelBuilder.Entity("LibBook.Domain.BookCategoryAgg.BookCategory", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("LibBook.Domain.PublisherAgg.Publisher", b =>
                {
                    b.Navigation("PublisherBooks");
                });

            modelBuilder.Entity("LibBook.Domain.TranslatorAgg.Translator", b =>
                {
                    b.Navigation("TranslatorBooks");
                });
#pragma warning restore 612, 618
        }
    }
}
