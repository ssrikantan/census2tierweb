using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using censusapp.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace censusapp.Controllers
{
    public class HomeController : Controller
    {
        ServiceNames sName;
        public HomeController(ServiceNames _sName)
        {
            sName = _sName;
        }
        public async Task<IActionResult> Family()
        {
            string l = HttpContext.Request.Host.Value;
            string sUri = string.Format("http://{0}/api/family", sName._sfWebApiServiceName);
            //string sUri = string.Format("http://{0}:{1}/api/family", sName._sfWebApiServiceName, sName._portno);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            List<Family> censusdata = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync(sUri);
               censusdata= await response.Content.ReadAsAsync<List<Family>>();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                ViewData["webapphost"] = error;

            }
            return View(censusdata);
        }

        public async Task<IActionResult> Index_old()
        {
            ViewData["Message"] = "Your application description page.";
            string l = HttpContext.Request.Host.Value;
            //string sUri = string.Format("http://{0}/api/values", sName._sfWebApiServiceName);
            string sUri = string.Format("http://{0}:{1}/api/values", sName._sfWebApiServiceName, sName._portno);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                HttpResponseMessage response = await client.GetAsync(sUri);
                string apiresponse = await response.Content.ReadAsStringAsync();
                ViewData["webapphost"] = "Host name of web app: " + HttpContext.Request.Host.Value;
                ViewData["webapihost"] = "Host name of API: " + apiresponse;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                ViewData["webapphost"] = error;

            }
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
