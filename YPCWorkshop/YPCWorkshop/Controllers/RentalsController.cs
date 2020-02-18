using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using YPCWorkshop.DAL;
using YPCWorkshop.Models;
using YPCWorkshop.ViewModels;

namespace YPCWorkshop.Controllers
{
    public class RentalsController : Controller
    {
        private YPCContext db = new YPCContext();
        // GET: Rentals
        public ActionResult Index()
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync("Rentals").Result;
            IEnumerable<Rental> rentals = response.Content.ReadAsAsync<IEnumerable<Rental>>().Result;
            response = WebClient.ApiClient.GetAsync("Volunteers").Result;
            IList<Volunteer> volunteers = response.Content.ReadAsAsync<IList<Volunteer>>().Result;
            response = WebClient.ApiClient.GetAsync("Tools").Result;
            IList<Tool> tools = response.Content.ReadAsAsync<IList<Tool>>().Result;

            var rentalsViewModel = rentals.Select(
                r => new RentalsViewModel
                {
                    RentalId = r.RentalId,
                    CheckOut = r.CheckOut,
                    CheckIn = r.CheckIn,
                    ToolName = tools.Where(t => t.ToolId == r.ToolId).Select(n => n.Name).FirstOrDefault(),
                    VolunteerName = volunteers.Where(v => v.VolunteerId == r.VolunteerId).Select(m => m.Name).FirstOrDefault()
                }).OrderByDescending(o => o.CheckOut).ToList();

            return View(rentalsViewModel);
        }

        public ActionResult Create()
        {
            var rental = new Rental();
            HttpResponseMessage response = WebClient.ApiClient.GetAsync("GetRentalMaxId").Result;
            // Setting the primary key value to a negative value will make SQL server to find the next available PKID when you save it.
            rental.RentalId = -999;
            rental.CheckOut = DateTime.Now;
            var volunteers = GetVolunteers();
            rental.Volunteers = volunteers;
            var tools = GetTools();
            rental.Tools = tools;
           // rental.Rented = new List<RentalsViewModel>();

            return View(rental);
        }

        [HttpPost]
        public ActionResult Create(Rental rental)
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.PostAsJsonAsync("Rentals", rental).Result;
                rental = response.Content.ReadAsAsync<Rental>().Result;

                // Change the tool.available to false in Tool table.
                response = WebClient.ApiClient.GetAsync($"Tools/{rental.ToolId}").Result;
                var tool = response.Content.ReadAsAsync<Tool>().Result;
                tool.Available = false;
                response = WebClient.ApiClient.PutAsJsonAsync($"Tools/{rental.ToolId}", tool).Result;
                
                TempData["SuccessMessage"] = "Saved successfully.";

                 return RedirectToAction("Index");                               
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int Id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Rentals/{Id}").Result;
            var rental = response.Content.ReadAsAsync<Rental>().Result;
            rental.CheckIn = DateTime.Now;

            return View(rental);
        }

        [HttpPost]
        public ActionResult Edit(int Id, Rental rental)// right click  Add view
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.PutAsJsonAsync($"Rentals/{Id}", rental).Result;
                response = WebClient.ApiClient.GetAsync($"Rentals/{Id}").Result;
                rental = response.Content.ReadAsAsync<Rental>().Result;

                response = WebClient.ApiClient.GetAsync($"Tools/{rental.ToolId}").Result;
                var tool = response.Content.ReadAsAsync<Tool>().Result;
                tool.Available = true;
                response = WebClient.ApiClient.PutAsJsonAsync($"Tools/{rental.ToolId}", tool).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Saved successfully.";

                    return RedirectToAction("Index");
                }
                return View(rental);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Details(int Id) 
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Rentals/{Id}").Result;
            var rental = response.Content.ReadAsAsync<Rental>().Result;

            return View(rental);
        }

        public ActionResult Delete(int Id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Rentals/{Id}").Result;
            var rental = response.Content.ReadAsAsync<Rental>().Result;

            return View(rental);
        }

        [HttpPost]
        public ActionResult Delete(int Id, Rental rental)
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Rentals/{Id}").Result;
                rental = response.Content.ReadAsAsync<Rental>().Result;

                response = WebClient.ApiClient.GetAsync($"Tools/{rental.ToolId}").Result;
                var tool = response.Content.ReadAsAsync<Tool>().Result;
                tool.Available = true;
                response = WebClient.ApiClient.PutAsJsonAsync($"Tools/{rental.ToolId}", tool).Result;
                
                response = WebClient.ApiClient.DeleteAsync($"Rentals/{Id}").Result;
                TempData["SuccessMessage"] = "Rental record deleted successfully.";
                return RedirectToAction("Index");
            }
            catch
            {
                return View(rental);
            }
        }

        public IEnumerable<SelectListItem> GetTools()
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync("Tools").Result;
            IList<Tool> dbTools = response.Content.ReadAsAsync<IList<Tool>>().Result;

            List<SelectListItem> tools = dbTools.Where(t => t.Available.Equals(true))
                                            .OrderBy(o => o.Name)
                                            .Select(m => new SelectListItem
                                            {
                                                Value = m.ToolId.ToString(),
                                                Text = " ID: " + m.ToolId.ToString() + " " + m.Name,
                                            }).ToList();

            return new SelectList(tools, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetVolunteers()
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync("Volunteers").Result;
            IList<Volunteer> dbVolunteers = response.Content.ReadAsAsync<IList<Volunteer>>().Result;
            List<SelectListItem> volunteers = dbVolunteers
                .OrderBy(o => o.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.VolunteerId.ToString(),
                    Text = " ID: " + c.VolunteerId.ToString() +" " + c.Name,
                }).ToList();

            return new SelectList(volunteers, "Value", "Text");
        }
    }
}