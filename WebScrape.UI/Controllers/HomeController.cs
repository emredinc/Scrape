using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebScrape.Manager;
using WebScrape.UI.Models;

namespace WebScrape.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IJobBoardService jobBoardService;


        public HomeController(ILogger<HomeController> logger, IJobBoardService _jobBoardService)
        {
            _logger = logger;
            jobBoardService = _jobBoardService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await jobBoardService.StartTheCrawler();
            if (response != null)
                jobBoardService.Add(response);
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
