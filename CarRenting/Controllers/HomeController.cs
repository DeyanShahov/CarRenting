using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarRenting.Data;
using CarRenting.Models;
using CarRenting.Models.Home;
using CarRenting.Services.Statistics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CarRenting.Controllers
{
    public class HomeController : Controller
    {
        private readonly CarRentingDbContext data;
        private readonly IStatisticsService statistics;
        private readonly IMapper mapper;

        public HomeController(IStatisticsService statistics, CarRentingDbContext data, IMapper mapper)
        {
            this.data = data;
            this.statistics = statistics;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            var cars = this.data
                .Cars
                .OrderByDescending(c => c.Id)
                .ProjectTo<CarIndexViewModel>(mapper.ConfigurationProvider)
                //.Select(c => new CarIndexViewModel
                //{
                //    Id = c.Id,
                //    Brand = c.Brand,
                //    Model = c.Model,
                //    Year = c.Year,
                //    Description = c.Description,
                //    ImageUrl = c.ImageUrl
                //})
                .Take(3)
                .ToList();

            var totalStatistics = statistics.Total();

            return View(new IndexViewModel
            {
                TotalCars = totalStatistics.TotalCars,
                TotalUsers = totalStatistics.TotalUsers,
                Cars = cars
            });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}