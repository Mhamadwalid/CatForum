﻿// <auto-generated />
using System;
using CatForum.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CatForum.Migrations
{
    // This class represents a snapshot of the database model, 
    // which helps Entity Framework track schema changes for migrations.
    [DbContext(typeof(CatForumContext))]
    partial class CatForumContextModelSnapshot : ModelSnapshot
    {
        // This method defines the structure of the database schema using Entity Framework's ModelBuilder.
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618 // Suppressing specific warnings related to obsolete APIs (if applicable).

            // Setting database annotations for versioning and identifier constraints.
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1") // Specifies the EF Core version.
                .HasAnnotation("Relational:MaxIdentifierLength", 128); // Sets the max identifier length for relational databases.

            // Enables identity column auto-increment behavior for primary keys (SQL Server specific).
            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            // Defining the "Post" entity (table)
            modelBuilder.Entity("CatForum.Models.Post", b =>
            {
                // Primary key with auto-increment.
                b.Property<int>("PostId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PostId"));

                // The main content of the post, which is required.
                b.Property<string>("Body")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                // Stores the timestamp of when the post was created.
                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("datetime2");

                // Optional image filename associated with the post.
                b.Property<string>("ImageFileName")
                    .HasColumnType("nvarchar(max)");

                // The title of the post, which is required.
                b.Property<string>("Title")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                // Sets the primary key of the "Posts" table.
                b.HasKey("PostId");

                // Maps the entity to the "Posts" table in the database.
                b.ToTable("Posts");
            });

            // Defining the "Reply" entity (table)
            modelBuilder.Entity("CatForum.Models.Reply", b =>
            {
                // Primary key with auto-increment.
                b.Property<int>("ReplyId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReplyId"));

                // Timestamp for when the reply was created.
                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("datetime2");

                // Foreign key linking the reply to a specific post.
                b.Property<int>("PostId")
                    .HasColumnType("int");

                // The text content of the reply, which is required.
                b.Property<string>("Text")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                // Sets the primary key of the "Replies" table.
                b.HasKey("ReplyId");

                // Creates an index on the PostId column for better query performance.
                b.HasIndex("PostId");

                // Maps the entity to the "Replies" table in the database.
                b.ToTable("Replies");
            });

            // Defining the relationship between Replies and Posts.
            modelBuilder.Entity("CatForum.Models.Reply", b =>
            {
                // A reply belongs to a single post, and when a post is deleted, its replies are also deleted (cascade delete).
                b.HasOne("CatForum.Models.Post", "Post")
                    .WithMany("Replies") // A post can have multiple replies.
                    .HasForeignKey("PostId")
                    .OnDelete(DeleteBehavior.Cascade) // Ensures replies are removed when their associated post is deleted.
                    .IsRequired();

                // Navigation property to access the related Post object.
                b.Navigation("Post");
            });

            // Configuring navigation property for a Post to reference its Replies.
            modelBuilder.Entity("CatForum.Models.Post", b =>
            {
                b.Navigation("Replies");
            });

#pragma warning restore 612, 618 // Re-enabling the previously suppressed warnings.
        }
    }
}
