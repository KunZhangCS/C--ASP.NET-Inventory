using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YPCWorkshop.ViewModels;

namespace YPCWorkshop.Models
{
    public class Rental
    {
        
        public int RentalId { get; set; }
        public DateTime CheckOut { get; set; }
        public DateTime? CheckIn { get; set; }
        public int VolunteerId { get; set; }
        public int ToolId { get; set; }

        public IEnumerable<SelectListItem> Volunteers { get; set; }
        public IEnumerable<SelectListItem> Tools { get; set; }
        public IEnumerable<RentalsViewModel> Rented { get; set; }

    }
}