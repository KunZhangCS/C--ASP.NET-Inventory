using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YPCWorkshop.ViewModels
{
    public class RentalsViewModel
    {
        public int RentalId { get; set; }
        public DateTime CheckOut { get; set; }
        public DateTime? CheckIn { get; set; }
        public string ToolName { get; set; }
        public string VolunteerName { get; set; }

        public IEnumerable<SelectListItem> Volunteers { get; set; }
        public IEnumerable<SelectListItem> Tools { get; set; }

    }
}