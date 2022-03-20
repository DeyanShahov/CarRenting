using CarRenting.Data;
using CarRenting.Data.Models;
using CarRenting.Models.Cars;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarRenting.Controllers
{
    public class CarsController : Controller
    {
        private readonly CarRentingDbContext data;

        public CarsController(CarRentingDbContext data)
        {
            this.data = data;
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!UserIsDealer())
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            return View(new AddCarFormModel
            {
                Categories = this.GetCarCategories()
            });
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

            if (!UserIsDealer())
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }


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
            var carsQuery = data.Cars.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                carsQuery = carsQuery.Where(c => c.Brand == query.Brand);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                carsQuery = carsQuery.Where(c =>
                    c.Brand.ToLower().Contains(query.SearchTerm.ToLower())
                    || c.Model.ToLower().Contains(query.SearchTerm.ToLower())
                    || c.Description.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            carsQuery = query.Sorting switch
            {
                CarSorting.DateCreated => carsQuery.OrderByDescending(c => c.Id),
                CarSorting.Year => carsQuery.OrderByDescending(c => c.Year),
                CarSorting.BrandAndModel => carsQuery.OrderBy(c => c.Brand).ThenBy(c => c.Model),
                _ => carsQuery.OrderByDescending(c => c.Id)
            };

            var cars = carsQuery
                .Skip((query.CurrentPage - 1) * AllCarsQueryModel.CarsPerPage)
                .Take(AllCarsQueryModel.CarsPerPage)
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

            var carBrands = data
                .Cars
                .Select(c => c.Brand)
                .Distinct()
                .OrderBy(br => br)
                .ToList();

            var totalCars = carsQuery.Count();

            query.TotalCars = totalCars;
            query.Brands = carBrands;
            query.Cars = cars;

            return View(query);
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


        private bool UserIsDealer()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userIsDealer = data
                .Dealers
                .Any(d => d.UserId == userId);

            return userIsDealer;
        }
    }
}
