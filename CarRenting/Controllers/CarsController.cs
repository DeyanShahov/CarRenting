using CarRenting.Data;
using CarRenting.Data.Models;
using CarRenting.Infrastructure;
using CarRenting.Models.Cars;
using CarRenting.Services.Cars;
using CarRenting.Services.Dealers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarRenting.Controllers
{
    public class CarsController : Controller
    {
        private readonly CarRentingDbContext data;

        private readonly IDealerService dealerSevice;

        private readonly ICarSevice carSevice;

        public CarsController(ICarSevice carSevice, CarRentingDbContext data, IDealerService dealerSevice)
        {
            this.data = data;
            this.carSevice = carSevice;
            this.dealerSevice = dealerSevice;
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!dealerSevice.IsDealer(User.GetId()))
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            return View(new AddCarFormModel
            {
                Categories = this.carSevice.AllCarCategories()
            }); ;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddCarFormModel carModel)
        {
            var dealerId = data
                .Dealers
                .Where(d => d.UserId == this.User.FindFirst(ClaimTypes.NameIdentifier).Value)
                .Select(d => d.Id)
                .FirstOrDefault();

            if (!dealerSevice.IsDealer(User.GetId()))
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }


            if (!data.Categories.Any(c => c.Id == carModel.CategoryId))
            {
                ModelState.AddModelError(nameof(carModel.CategoryId), "Category");
            }


            if (!ModelState.IsValid)
            {
                carModel.Categories = this.carSevice.AllCarCategories();

                return View(carModel);
            }


            var car = new Car
            {
                Brand = carModel.Brand,
                Model = carModel.Model,
                Description = carModel.Description,
                ImageUrl = carModel.ImageUrl,
                Year = carModel.Year,
                CategoryId = carModel.CategoryId,
                DealerId = dealerId,
            };

            data.Cars.Add(car);

            data.SaveChanges();

            //return RedirectToAction("Index", "Home");        
            return RedirectToAction(nameof(All));
        }


        public IActionResult All([FromQuery] AllCarsQueryModel query)
        {
            var queryResult = carSevice.All(
                query.Brand,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllCarsQueryModel.CarsPerPage);

            var carBrands = carSevice.AllCarBrands();
         
            query.TotalCars = queryResult.TotalCars;
            query.Brands = carBrands;
            query.Cars = queryResult.Cars;

            return View(query);
        }


        [Authorize]
        public IActionResult Mine()
        {
            var myCars = carSevice.ByUser(this.User.GetId());

            return View(myCars);
        }

        [Authorize]
        public IActionResult Edit()
        {

        }

    }
}
