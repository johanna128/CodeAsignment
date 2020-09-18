using CodeAsignment.Models;
using CodeAsignment.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CodeAsignment.Controllers
{
    public class HomeController : Controller
    {
        private string userKey = "OWUyYWM5ZGQtMjI0NC00ZWU4LWJjZWUtMjE5N2NhMzcxY2E5";

        public ActionResult Index(HomeModel model)
        {
            ViewBag.Title = "Home Page";

            return View(model);
        }
        [HttpPost]
        public ActionResult ViewApps(HomeModel model)
        {
            if (ModelState.IsValid)
            {
                WebRequest request = WebRequest.Create(model.oneSignalURL);
                request.Method = "GET";
                request.Headers.Add("Authorization", string.Format("{0} {1}", "Basic", userKey));
                request.ContentType = "application/json";

                var myWebResponse = request.GetResponse();
                var responseStream = myWebResponse.GetResponseStream();
                if (responseStream == null) return null;

                var myStreamReader = new StreamReader(responseStream, Encoding.Default);
                var json = myStreamReader.ReadToEnd();
                model.apiResponsMessage = json;

                responseStream.Close();
                myWebResponse.Close();
            }
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult CreateApp(HomeModel model)
        {
            var viewModel = new HomeViewModel();

            if (ModelState.IsValid)
            {
                viewModel.name = Request.Form["name"];
                viewModel.chrome_web_origin = Request.Form["chrome_web_origin"];

                WebRequest request = WebRequest.Create(model.oneSignalURL);
                request.Method = "POST";
                request.Headers.Add("Authorization", string.Format("{0} {1}", "Basic", userKey));
                request.ContentType = "application/json";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string jsonString = JsonConvert.SerializeObject(viewModel);
                    streamWriter.Write(jsonString);
                }

                var myWebResponse = request.GetResponse();
                var responseStream = myWebResponse.GetResponseStream();
                if (responseStream == null) return null;

                var myStreamReader = new StreamReader(responseStream, Encoding.Default);
                var json = myStreamReader.ReadToEnd();
                model.apiResponsMessage = json;

                responseStream.Close();
                myWebResponse.Close();
            }
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult UpdateApp(HomeModel model)
        {
            var viewModel = new HomeViewModel();

            if (ModelState.IsValid)
            {
                viewModel.name = Request.Form["name"];
                viewModel.chrome_web_origin = Request.Form["chrome_web_origin"];

                WebRequest request = WebRequest.Create(string.Format("{0}/{1}", model.oneSignalURL, Request.Form["appID"]));
                request.Method = "PUT";
                request.Headers.Add("Authorization", string.Format("{0} {1}", "Basic", userKey));
                request.ContentType = "application/json";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string jsonString = JsonConvert.SerializeObject(viewModel);
                    streamWriter.Write(jsonString);
                }

                var myWebResponse = request.GetResponse();
                var responseStream = myWebResponse.GetResponseStream();
                if (responseStream == null) return null;

                var myStreamReader = new StreamReader(responseStream, Encoding.Default);
                var json = myStreamReader.ReadToEnd();
                model.apiResponsMessage = json;

                responseStream.Close();
                myWebResponse.Close();
            }
            return View("Index", model);
        }
    }
}
