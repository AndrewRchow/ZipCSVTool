using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Ionic.Zip;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ZipCSVTool.Models;

namespace ZipCSVTool.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
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


        [HttpPost]
        public async Task<ActionResult<byte[]>> ZipAndDownload(Guid caseId, string fileName, string finalFolder)
        {
            string contentRootPath = _hostingEnvironment.ContentRootPath + "\\Content\\Csv";

            using (var zipFile = new ZipFile())
            {
                zipFile.AddDirectory(contentRootPath, "");

                var pushStreamContent = new PushStreamContent((stream, content, context) =>
                {
                    zipFile.Save(stream);
                    stream.Close(); 
                }, "application/zip");

                return await pushStreamContent.ReadAsByteArrayAsync();

            }
        }


    }

}