using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ConsoleApp7
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Passenger> Passengers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Perevozka;Username=postgres;Password=Packardbell345");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Driver>().HasKey(d => d.DriverID);
            modelBuilder.Entity<Route>().HasKey(r => r.RouteID);
            modelBuilder.Entity<Vehicle>().HasKey(v => v.VehicleID);
            modelBuilder.Entity<Ticket>().HasKey(t => t.TicketID);
            modelBuilder.Entity<Passenger>().HasKey(p => p.PassengerID);

            // Определение ключей и связей
            modelBuilder.Entity<Driver>()
                .HasKey(d => d.DriverID);

            modelBuilder.Entity<Route>()
                .HasKey(r => r.RouteID);

            modelBuilder.Entity<Vehicle>()
                .HasKey(v => v.VehicleID);

            modelBuilder.Entity<Ticket>()
                .HasKey(t => t.TicketID);

            modelBuilder.Entity<Passenger>()
                .HasKey(p => p.PassengerID);

            // Определение связей
            modelBuilder.Entity<Driver>()
                .HasOne(d => d.Vehicle)
                .WithOne(v => v.Driver)
                .HasForeignKey<Driver>(d => d.VehicleID);

            modelBuilder.Entity<Route>()
                .HasOne(r => r.Vehicle)
                .WithMany(v => v.Routes)
                .HasForeignKey(r => r.VehicleID);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Passenger)
                .WithMany(p => p.Tickets)
                .HasForeignKey(t => t.PassengerID);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Vehicle)
                .WithMany(v => v.Tickets)
                .HasForeignKey(t => t.VehicleID);
        }
    }
    }
   