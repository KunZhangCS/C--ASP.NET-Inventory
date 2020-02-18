using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using YPCWorkshop.DAL;
using YPCWorkshop.Models;

namespace YPCWorkshop.Controllers
{
    public class VolunteersController : Controller
    {
        private YPCContext db = new YPCContext();
        // GET: Volunteers
        public ActionResult Index()
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync("Volunteers").Result;
            IEnumerable<Volunteer> volunteers = response.Content.ReadAsAsync<IEnumerable<Volunteer>>().Result;
            return View(volunteers);
        }

        public ActionResult Edit(int Id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Volunteers/{Id}").Result;
            var volunteer = response.Content.ReadAsAsync<Volunteer>().Result;

            return View(volunteer);
        }

        [HttpPost]
        public ActionResult Edit(int Id, Volunteer volunteer)// right click  Add view
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.PutAsJsonAsync($"Volunteers/{Id}", volunteer).Result;

                if (response.IsSuccessStatusCode)
                {
                    // we will refer to this in the Index.cshtml of the Volunteer so Alertify can display the message
                    TempData["SuccessMessage"] = "Saved successfully.";

                    return RedirectToAction("Index");
                }
                return View(volunteer);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        // This is the Create Post to create new movie
        [HttpPost]
        public ActionResult Create(Volunteer volunteer) // model create
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.PostAsJsonAsync("Volunteers", volunteer).Result;

                // we will refer to this in the Index.cshtml of the Volunteer so Alertify can display the message
                TempData["SuccessMessage"] = "Volunteer added successfully.";

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Details(int Id) // model details
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Volunteers/{Id}").Result;
            var volunteer = response.Content.ReadAsAsync<Volunteer>().Result;

            return View(volunteer);
        }

        public ActionResult Delete(int Id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Volunteers/{Id}").Result;
            var volunteer = response.Content.ReadAsAsync<Volunteer>().Result;
            return View(volunteer);
        }

        [HttpPost]
        public ActionResult Delete(FormCollection collection, int Id)
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.DeleteAsync($"Volunteers/{Id}").Result;

                // we will refer to this in the Index.cshtml of the Volunteer so Alertify can display the message
                TempData["SuccessMessage"] = "Volunteer deleted successfully.";

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}