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
    public class ToolsController : Controller
    {
        private YPCContext db = new YPCContext();
        // GET: Tools
        public ActionResult Index()
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync("Tools").Result;
            IEnumerable<Tool> tools = response.Content.ReadAsAsync<IEnumerable<Tool>>().Result;
            return View(tools);
        }

        public ActionResult Edit(int Id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Tools/{Id}").Result;
            var tool = response.Content.ReadAsAsync<Tool>().Result;
            
            return View(tool);
        }
        
        [HttpPost] 
        public ActionResult Edit(int Id, Tool tool)// right click  Add view
        {
            try
            {
                if (tool.Active.Equals(false))
                {
                    tool.Available = false;
                }
                HttpResponseMessage response = WebClient.ApiClient.PutAsJsonAsync($"Tools/{Id}", tool).Result;

                if (response.IsSuccessStatusCode)
                {
                    // we will refer to this in the Index.cshtml of the Tool so Alertify can display the message
                    TempData["SuccessMessage"] = "Saved successfully.";

                    return RedirectToAction("Index");
                }
                return View(tool);
            }
            catch
            {
                return View();
            }
        }
         
        public ActionResult Create()
        {
            var tool = new Tool();
            tool.Active = true;
            tool.Available = true;
            return View(tool);
        }

        // This is the Create Post to create new movie
        [HttpPost]
        public ActionResult Create(Tool tool) // model create
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.PostAsJsonAsync("Tools", tool).Result;
                
                // we will refer to this in the Index.cshtml of the Tool so Alertify can display the message
                TempData["SuccessMessage"] = "Tool added successfully.";

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Details(int Id) // model details
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Tools/{Id}").Result;
            var tool = response.Content.ReadAsAsync<Tool>().Result;

            return View(tool);
        }
         
        public ActionResult Delete(int Id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Tools/{Id}").Result;
            var tool = response.Content.ReadAsAsync<Tool>().Result;
            return View(tool);
        }
         
        [HttpPost]
        public ActionResult Delete(FormCollection collection, int Id) 
        {
            try
            { 
                HttpResponseMessage response = WebClient.ApiClient.DeleteAsync($"Tools/{Id}").Result;

                // we will refer to this in the Index.cshtml of the Tool so Alertify can display the message
                TempData["SuccessMessage"] = "Tool deleted successfully.";

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}