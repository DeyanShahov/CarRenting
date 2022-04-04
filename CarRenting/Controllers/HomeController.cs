using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarRenting.Data;
using CarRenting.Models;
using CarRenting.Models.Home;
using CarRenting.Services.Cars;
using CarRenting.Services.Statistics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CarRenting.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICarSevice carsService;
        private readonly IStatisticsService statistics;

        public HomeController(IStatisticsService statistics, ICarSevice carsService)
        {

            this.statistics = statistics;
            this.carsService = carsService;
        }

        public IActionResult Index()
        {
            var latestCars = carsService.Latest().ToList();

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