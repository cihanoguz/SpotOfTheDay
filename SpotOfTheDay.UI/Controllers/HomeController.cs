using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SpotOfTheDay.UI.Business;
using SpotOfTheDay.UI.Business.CacheManager;
using SpotOfTheDay.UI.Models;
using SpotOfTheDay.UI.Models.Response;
using SpotOfTheDay.UI.Models.Response.AdVisionService;
using SpotOfTheDay.UI.Services;

namespace SpotOfTheDay.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ICacheManager _cacheService;

        public HomeController(
            IConfiguration configuration,
            ICacheManager cacheService)
        {
            _configuration = configuration;
            _cacheService = cacheService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            AdVisionManager adVisionManager = new AdVisionManager(_configuration, _cacheService);
            BaseResponse<GetAllResponse> spotOfTheDayDataList = await adVisionManager.GetDataFromSpotOfTheDay(page);

            return View(spotOfTheDayDataList);
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

