using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.Models
{
    // Required Packages
    // Microsoft.EntityFrameworkCore
    // Microsoft.EntityFrameworkCore.SqlServer
    // Microsoft.EntityFrameworkCore.Tools
    public class AppointmentDBContext:DbContext
    {
        public AppointmentDBContext(DbContextOptions<AppointmentDBContext> options):base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserStoreRole> UserStoreRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<StoreHours> StoreHours { get; set; }
        public DbSet<ClosedDaysTimes> ClosedDaysTimes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User Table
            modelBuilder.Entity<User>()
                .HasIndex(u => u.email).IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.username).IsUnique();
            modelBuilder.Entity<User>()
                .Property(e => e.createdAt)
                .HasDefaultValueSql("NOW()")
                .ValueGeneratedOnAdd();

            // Store Table
            modelBuilder.Entity<Store>()
                .Property(e => e.createdAt)
                .HasDefaultValueSql("NOW()")
                .ValueGeneratedOnAdd();

            // Role Table
            modelBuilder.Entity<Role>()
                .HasIndex(r => r.name).IsUnique();

            modelBuilder.Entity<Role>()
                .HasData(
                    new Role
                    {
                        id = 1,
                        name = "Owner",
                        description = "Owner/Creator of the store"
                    },
                    new Role
                    {
                        id = 2,
                        name = "Manager",
                        description = "Manager of the store and appointment times"
                    });

            // Permission Table
            modelBuilder.Entity<Permission>()
                .HasIndex(p => p.name).IsUnique();

            modelBuilder.Entity<Permission>()
                .HasData(
                    new Permission
                    {
                        id = 1,
                        name = "Delete Store"
                    },
                    new Permission
                    {
                        id = 2,
                        name = "Edit Store Details"
                    },
                    new Permission
                    {
                        id = 3,
                        name = "Edit Store Hours"
                    },
                    new Permission
                    {
                        id = 4,
                        name = "Edit Closed Times"
                    },
                    new Permission
                    {
                        id = 5,
                        name = "Assign Managers"
                    });

            // UserStoreRole Table
            modelBuilder.Entity<UserStoreRole>()
                .HasKey(x => new { x.userId, x.storeId, x.roleId });

            modelBuilder.Entity<UserStoreRole>()
                .Property(e => e.createdAt)
                .HasDefaultValueSql("NOW()")
                .ValueGeneratedOnAdd();

            // RolePermission Table
            modelBuilder.Entity<RolePermission>()
                .HasKey(x => new { x.roleId, x.permissionId });

            modelBuilder.Entity<RolePermission>()
                .HasData(
                    new RolePermission 
                    { 
                        roleId = 1,
                        permissionId = 1
                    },
                    new RolePermission
                    {
                        roleId = 1,
                        permissionId = 2
                    },
                    new RolePermission
                    {
                        roleId = 1,
                        permissionId = 3
                    },
                    new RolePermission
                    {
                        roleId = 1,
                        permissionId = 4
                    },
                    new RolePermission
                    {
                        roleId = 1,
                        permissionId = 5
                    },
                    new RolePermission
                    {
                        roleId = 2,
                        permissionId = 3
                    }, 
                    new RolePermission
                    {
                        roleId = 2,
                        permissionId = 4
                    });

            // StoreHours Table
            modelBuilder.Entity<StoreHours>()
                .HasKey(h => new { h.storeId, h.dayOfWeek });

            // ClosedDaysTimes Table
            modelBuilder.Entity<ClosedDaysTimes>()
                .Property(c => c.repeatInterval)
                .HasConversion<int>();

            // Customer Table

            // Appointment Table
            modelBuilder.Entity<Appointment>()
                .Property(a => a.status)
                .HasConversion<int>();

            modelBuilder.Entity<Appointment>()
                .Property(e => e.createdAt)
                .HasDefaultValueSql("NOW()")
                .ValueGeneratedOnAdd();

        }



    }
}
