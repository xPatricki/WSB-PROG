using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace biblioteka.ViewModels
{
    public class ReservationViewModel
    {
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

        public List<SelectListItem> Users { get; set; }
        public List<SelectListItem> Books { get; set; }
    }
}
