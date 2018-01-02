using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace MnistWeb.Controllers
{
    [Route("mnist")]
    public class MnistController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public MnistController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var path = $"/index.html";
            return File(path, $"text/html");
        }
    }
}