using CarRenting.Data;
using CarRenting.Data.Models;
using CarRenting.Models.Cars;
using Microsoft.AspNetCore.Mvc;

namespace CarRenting.Controllers
{
    public class CarsController : Controller
    {
        private readonly CarRentingDbContext data;

        public CarsController(CarRentingDbContext data)
        {
            this.data = data;
        }

        public IActionResult Add() => View(new AddCarFormModel
        {
            Categories = this.GetCarCategories()
        });

        [HttpPost]
        public IActionResult Add(AddCarFormModel carModel)
        {
            if (!data.Categories.Any(c => c.Id == carModel.CategoryId))
            {
                ModelState.AddModelError(nameof(carModel.CategoryId), "Category");
            }


            if (!ModelState.IsValid)
            {
                carModel.Categories = this.GetCarCategories();

                return View(carModel);  
            }


            var car = new Car
            {
                Brand = carModel.Brand,
                Model = carModel.Model,
                Description = carModel.Description,
                ImageUrl = carModel.ImageUrl,
                Year = carModel.Year,
                CategoryId = carModel.CategoryId
            };

            data.Cars.Add(car);

            data.SaveChanges();
            
            //return RedirectToAction("Index", "Home");        
            return RedirectToAction(nameof(All));
        }


        public IActionResult All()
        {
            var Cars = this.data
                .Cars
                .OrderByDescending(c => c.Id )
                .Select(c => new CarListingViewModel
                {
                    Id = c.Id,
                    Brand = c.Brand,
                    Model = c.Model,
                    Year = c.Year,
                    Description = c.Description,
                    ImageUrl = c.ImageUrl,
                    Category = c.Category.Name
                })
                .ToList();

            return View(Cars);
        }

        private IEnumerable<CarCategoryViewModel> GetCarCategories()
        {
            return this.data
                    .Categories
                    .Select(c => new CarCategoryViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                    })
                    .ToList();
        }
    }
}
