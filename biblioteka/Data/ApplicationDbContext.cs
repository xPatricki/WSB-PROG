using Microsoft.EntityFrameworkCore;
using biblioteka.Models.DBEntities;

namespace biblioteka.Data
{
    public class BooksDbContext : DbContext
    {
        public BooksDbContext(DbContextOptions<BooksDbContext> options)
            : base(options)
        {
        }

        public DbSet<Books> Books { get; set; }
    }

    public class ReservationsDbContext : DbContext
    {
        public ReservationsDbContext(DbContextOptions<ReservationsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Reservations> Reservations { get; set; }
    }
}
