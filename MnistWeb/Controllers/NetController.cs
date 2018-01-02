using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MnistWeb.Repository;
using MnistWeb.Services;
using Microsoft.AspNetCore.Hosting;

namespace MnistWeb.Controllers
{
    [Produces("application/json")]
    [Route("api/nets")]
    public class NetController : Controller
    {
        private readonly INetRepository _netRepository;
        private readonly IMnistService _mnistService;
        private readonly IHostingEnvironment _hostingEnvironment;
        public NetController(
            INetRepository netRepository, 
            IMnistService mnistService,
            IHostingEnvironment hostingEnvironment)
        {
            _netRepository = netRepository;
            _mnistService = mnistService;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet("net/{id}")]
        public IActionResult GetNet(int id)
        {
            var net = _netRepository.GetSingle(id);
            if (net == null)
            {
                return NotFound();
            }

            return Ok(net);
        }

        [HttpGet("testimage")]
        public IActionResult GetTestImage()
        {
            var path = $"/imgs/numbers.jpg";
            var filepath = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "imgs", "numbers.jpg");

            if (!System.IO.File.Exists(filepath))
            {
                return NotFound();
            }
            return File(path, $"image/jpg");
        }

        [HttpPost("addnet")]
        public IActionResult PostNet([FromBody]DBModels.Net net)
        {
            if (net == null)
            {
                return BadRequest();
            }
            _netRepository.Add(net);
            if (_netRepository.Save())
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("prediction")]
        public IActionResult PostImage([FromBody]Models.Image img)
        {
            if (img == null)
            {
                return BadRequest();
            }
            var res = _mnistService.Calculate(img.Bytes.ToArray());
            return Ok(res);
        }

    }
}