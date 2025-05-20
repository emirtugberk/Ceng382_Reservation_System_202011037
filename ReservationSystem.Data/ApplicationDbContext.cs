using Microsoft.EntityFrameworkCore;
using ReservationSystem.Data.Entities;

namespace ReservationSystem.Data
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {}

        public DbSet<Term> Terms { get; set; }

        public DbSet<User> Users { get; set;}

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<LogEntry> LogEntries { get; set; }
    }
}