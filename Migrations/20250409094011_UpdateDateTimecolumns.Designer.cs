﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace api_for_sambapos.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250409094011_UpdateDateTimecolumns")]
    partial class UpdateDateTimecolumns
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("api_for_sambapos.Models.MenuItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("GroupCode")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("GroupCode");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MenuItems");
                });

            modelBuilder.Entity("api_for_sambapos.Models.Tables", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Tables");
                });

            modelBuilder.Entity("api_for_sambapos.Models.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CustomerGroupCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastOrderDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("LastPaymentDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("LocationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Locked")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrintJobData")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("RemainingAmount")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TicketNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("Id");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("api_for_sambapos.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PinCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserRole_Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
