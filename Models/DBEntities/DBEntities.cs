using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace biblioteka.Models.DBEntities
{
    public class Books
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        public string ISBN { get; set; }

        public int PublishedYear { get; set; }

        public int AvailableCopies { get; set; }

        public ICollection<Reservations> Reservations { get; set; }
    }
    public class Reservations
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ReservationID { get; set; }
        public string UserID { get; set; }
        public IdentityUser User { get; set; }
        public int BookID { get; set; }
        public Books Book { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime DueDate { get; set; }
    }
    public class PagerOptions
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int PageCount => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}
