using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using biblioteka.Models;
using biblioteka.Models.DBEntities;

namespace biblioteka.Models
{
    public class BooksViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int PublishedYear { get; set; }
        public int AvailableCopies { get; set; }
        public ICollection<Reservations> Reservations { get; set; }
    }
    public class ReservationsViewModel
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
}
