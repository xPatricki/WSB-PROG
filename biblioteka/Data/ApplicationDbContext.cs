using Microsoft.EntityFrameworkCore;
using biblioteka.Models.DBEntities;
using biblioteka.Models;

namespace biblioteka.Data
{
    public class BooksDbContext : DbContext
    {
        public BooksDbContext(DbContextOptions<BooksDbContext> options)
            : base(options)
        {
        }

        public DbSet<Books> Books { get; set; }
        public DbSet<biblioteka.Models.BooksViewModel> BooksViewModel { get; set; } = default!;
    }

    public class ReservationsDbContext : DbContext
    {
        public ReservationsDbContext(DbContextOptions<ReservationsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Reservations> Reservations { get; set; }

public DbSet<biblioteka.Models.ReservationsViewModel> ReservationsViewModel { get; set; } = default!;
    }
}
