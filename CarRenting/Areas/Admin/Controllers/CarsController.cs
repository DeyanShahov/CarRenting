using CarRenting.Services.Cars;
using Microsoft.AspNetCore.Mvc;

namespace CarRenting.Areas.Admin.Controllers
{
    public class CarsController : AdminController
    {
        private readonly ICarSevice carsService;

        public CarsController(ICarSevice carsService)
        {
            this.carsService = carsService;
        }

        public IActionResult All()
        {
            var cars = carsService
                .All(publicOnly: false)
                .Cars;

            return View(cars);
        }

        public IActionResult ChangeVisibility(int id)
        {
            carsService.ChangeVisibility(id);

            return RedirectToAction(nameof(All));
        }
    }
}
