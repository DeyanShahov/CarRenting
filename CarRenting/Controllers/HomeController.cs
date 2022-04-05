using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarRenting.Data;
using CarRenting.Models;
using CarRenting.Models.Home;
using CarRenting.Services.Cars;
using CarRenting.Services.Statistics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace CarRenting.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICarSevice carsService;
        private readonly IStatisticsService statistics;
        private readonly IMemoryCache cache;

        public HomeController(IStatisticsService statistics, ICarSevice carsService, IMemoryCache cache)
        {

            this.statistics = statistics;
            this.carsService = carsService;
            this.cache = cache;
        }

        public IActionResult Index()
        {
            const string latestCarsCacheKey = "LatestCarsCaheKey";

            var latestCars = cache.Get<List<LatestCarServiceModel>>(latestCarsCacheKey);

            if (latestCars == null)
            {
                latestCars = carsService.Latest().ToList();

                var cacheOption = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(5));

                this.cache.Set(latestCarsCacheKey, latestCars, cacheOption);
            }
          
            var totalStatistics = statistics.Total();

            return View(new IndexViewModel
            {
                TotalCars = totalStatistics.TotalCars,
                TotalUsers = totalStatistics.TotalUsers,
                Cars = latestCars
            });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}