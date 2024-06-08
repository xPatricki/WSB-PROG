using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using biblioteka.Models;
using biblioteka.Models.DBEntities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
    
    [Required]
    public string UserID { get; set; }

    [Required]
    public int BookID { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime ReservationDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime DueDate { get; set; }
}

}
