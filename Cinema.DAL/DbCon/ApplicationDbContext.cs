using Microsoft.EntityFrameworkCore;
using Cinema.Core.Models;
using System;

namespace Cinema.DAL.DbCon
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
