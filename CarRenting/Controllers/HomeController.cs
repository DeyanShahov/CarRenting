using CarRenting.Models;
using CarRenting.Services.Cars;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace CarRenting.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICarSevice carsService;
        private readonly IMemoryCache cache;

        public HomeController(ICarSevice carsService, IMemoryCache cache)
        {
            this.carsService = carsService;
            this.cache = cache;
        }

        public IActionResult Index()
        {
            const string latestCarsCacheKey = "LatestCarsCaheKey";

            //Add sesion information from current user
            this.HttpContext.Session.SetString("KeyParameter", "ValueParameter");

            var latestCars = cache.Get<List<LatestCarServiceModel>>(latestCarsCacheKey);

            if (latestCars == null)
            {
                latestCars = carsService.Latest().ToList();

                var cacheOption = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(5));

                this.cache.Set(latestCarsCacheKey, latestCars, cacheOption);
            }       

            return View(latestCars);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}