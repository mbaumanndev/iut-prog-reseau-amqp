using IutInfo.ProgReseau.BuildBlocks.RabbitMQ;
using IutInfo.ProgReseau.RabbitClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace IutInfo.ProgReseau.RabbitClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("/post")]
        public IActionResult Post([FromServices] IRabbitManager p_Manager, [FromForm] FormObject p_Form)
        {
            p_Manager.Publish(p_Form.Text, "server.exchange", "topic", "server.queue.*");

            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public class FormObject
        {
            public string Text { get; set; }
        }
    }
}