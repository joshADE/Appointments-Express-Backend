﻿// <auto-generated />
using System;
using Appointments_Express_Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Appointments_Express_Backend.Migrations
{
    [DbContext(typeof(AppointmentDBContext))]
    [Migration("20210623023008_QuickProfileAndClosedStoreHours")]
    partial class QuickProfileAndClosedStoreHours
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Appointments_Express_Backend.Models.Appointment", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("createdAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<int>("customerId")
                        .HasColumnType("integer");

                    b.Property<string>("description")
                        .HasColumnType("text");

                    b.Property<DateTime>("end")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("start")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("status")
                        .HasColumnType("integer");

                    b.Property<int>("storeId")
                        .HasColumnType("integer");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("customerId");

                    b.HasIndex("storeId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("Appointments_Express_Backend.Models.ClosedDaysTimes", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("from")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("repeat")
                        .HasColumnType("boolean");

                    b.Property<int?>("repeatInterval")
                        .HasColumnType("integer");

                    b.Property<int>("storeId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("to")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.HasIndex("storeId");

                    b.ToTable("ClosedDaysTimes");
                });

            modelBuilder.Entity("Appointments_Express_Backend.Models.Customer", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("firstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("lastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Appointments_Express_Backend.Models.Permission", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("name")
                        .IsUnique();

                    b.ToTable("Permissions");

                    b.HasData(
                        new
                        {
                            id = 1,
                            name = "Delete Store"
                        },
                        new
                        {
                            id = 2,
                            name = "Edit Store Details"
                        },
                        new
                        {
                            id = 3,
                            name = "Edit Store Hours"
                        },
                        new
                        {
                            id = 4,
                            name = "Edit Closed Times"
                        },
                        new
                        {
                            id = 5,
                            name = "Assign Managers"
                        });
                });

            modelBuilder.Entity("Appointments_Express_Backend.Models.Role", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("description")
                        .HasColumnType("text");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("name")
                        .IsUnique();

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            id = 1,
                            description = "Owner/Creator of the store",
                            name = "Owner"
                        },
                        new
                        {
                            id = 2,
                            description = "Manager of the store and appointment times",
                            name = "Manager"
                        });
                });

            modelBuilder.Entity("Appointments_Express_Backend.Models.RolePermission", b =>
                {
                    b.Property<int>("roleId")
                        .HasColumnType("integer");

                    b.Property<int>("permissionId")
                        .HasColumnType("integer");

                    b.HasKey("roleId", "permissionId");

                    b.HasIndex("permissionId");

                    b.ToTable("RolePermissions");

                    b.HasData(
                        new
                        {
                            roleId = 1,
                            permissionId = 1
                        },
                        new
                        {
                            roleId = 1,
                            permissionId = 2
                        },
                        new
                        {
                            roleId = 1,
                            permissionId = 3
                        },
                        new
                        {
                            roleId = 1,
                            permissionId = 4
                        },
                        new
                        {
                            roleId = 1,
                            permissionId = 5
                        },
                        new
                        {
                            roleId = 2,
                            permissionId = 3
                        },
                        new
                        {
                            roleId = 2,
                            permissionId = 4
                        });
                });

            modelBuilder.Entity("Appointments_Express_Backend.Models.Store", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("createdAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<bool>("isQuickProfile")
                        .HasColumnType("boolean");

                    b.Property<string>("location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("maxTimeBlock")
                        .HasColumnType("integer");

                    b.Property<int>("minTimeBlock")
                        .HasColumnType("integer");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("Appointments_Express_Backend.Models.StoreHours", b =>
                {
                    b.Property<int>("storeId")
                        .HasColumnType("integer");

                    b.Property<int>("dayOfWeek")
                        .HasColumnType("integer");

                    b.Property<TimeSpan>("close")
                        .HasColumnType("interval");

                    b.Property<bool>("isOpen")
                        .HasColumnType("boolean");

                    b.Property<TimeSpan>("open")
                        .HasColumnType("interval");

                    b.HasKey("storeId", "dayOfWeek");

                    b.ToTable("StoreHours");
                });

            modelBuilder.Entity("Appointments_Express_Backend.Models.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("createdAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("firstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("lastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("email")
                        .IsUnique();

                    b.HasIndex("username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Appointments_Express_Backend.Models.UserStoreRole", b =>
                {
                    b.Property<int>("userId")
                        .HasColumnType("integer");

                    b.Property<int>("storeId")
                        .HasColumnType("integer");

                    b.Property<int>("roleId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("createdAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("NOW()");

                    b.HasKey("userId", "storeId", "roleId");

                    b.HasIndex("roleId");

                    b.HasIndex("storeId");

                    b.ToTable("UserStoreRoles");
                });

            modelBuilder.Entity("Appointments_Express_Backend.Models.Appointment", b =>
                {
                    b.HasOne("Appointments_Express_Backend.Models.Customer", "customer")
                        .WithMany()
                        .HasForeignKey("customerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Appointments_Express_Backend.Models.Store", "store")
                        .WithMany()
                        .HasForeignKey("storeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("customer");

                    b.Navigation("store");
                });

            modelBuilder.Entity("Appointments_Express_Backend.Models.ClosedDaysTimes", b =>
                {
                    b.HasOne("Appointments_Express_Backend.Models.Store", "store")
                        .WithMany()
                        .HasForeignKey("storeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("store");
                });

            modelBuilder.Entity("Appointments_Express_Backend.Models.RolePermission", b =>
                {
                    b.HasOne("Appointments_Express_Backend.Models.Permission", "permission")
                        .WithMany()
                        .HasForeignKey("permissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Appointments_Express_Backend.Models.Role", "role")
                        .WithMany()
                        .HasForeignKey("roleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("permission");

                    b.Navigation("role");
                });

            modelBuilder.Entity("Appointments_Express_Backend.Models.StoreHours", b =>
                {
                    b.HasOne("Appointments_Express_Backend.Models.Store", "store")
                        .WithMany()
                        .HasForeignKey("storeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("store");
                });

            modelBuilder.Entity("Appointments_Express_Backend.Models.UserStoreRole", b =>
                {
                    b.HasOne("Appointments_Express_Backend.Models.Role", "role")
                        .WithMany()
                        .HasForeignKey("roleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Appointments_Express_Backend.Models.Store", "store")
                        .WithMany()
                        .HasForeignKey("storeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Appointments_Express_Backend.Models.User", "user")
                        .WithMany()
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("role");

                    b.Navigation("store");

                    b.Navigation("user");
                });
#pragma warning restore 612, 618
        }
    }
}
