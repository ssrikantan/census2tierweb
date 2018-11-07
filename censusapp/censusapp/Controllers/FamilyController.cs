using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using censusapp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace censusapp.Controllers
{
    public class FamilyController : Controller
    {
        private ServiceNames sName;
        private string sUri;
        public FamilyController(ServiceNames _sName)
        {
            sName = _sName;
            //sUri = string.Format("http://{0}/api/family", sName._sfWebApiServiceName);
            sUri = string.Format("http://{0}:{1}/api/family", sName._sfWebApiServiceName, sName._portno);
        }


        // GET: Family
        public async Task<ActionResult> Index()
        {
            string l = HttpContext.Request.Host.Value;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            List<Family> censusdata = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync(sUri);
                censusdata = await response.Content.ReadAsAsync<List<Family>>();
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Error));

            }
            return View(censusdata);

        }

        // GET: Family/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Family/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Family/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Family censusdata)
        {
            try
            {
                // TODO: Add insert logic here
                string l = HttpContext.Request.Host.Value;
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync<Family>(sUri,censusdata);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Error));
            }
        }

        // GET: Family/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Family/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Family/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Family/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}