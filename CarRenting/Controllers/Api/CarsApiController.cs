using CarRenting.Models.Api;
using CarRenting.Services.Cars;
using Microsoft.AspNetCore.Mvc;

namespace CarRenting.Controllers.Api
{

    [ApiController]
    [Route("api/cars")]
    public class CarsApiController : ControllerBase
    {
        private readonly ICarSevice carSevice;

        public CarsApiController(ICarSevice carSevice)
        {
            this.carSevice = carSevice;
        }

        //test
        //[HttpGet]
        //public IEnumerable<string> GetCar()
        //{           
        //    return carSevice.AllCarBrands();
        //}

        //test
        //[HttpGet]
        //[Route("{id}")]
        //public IActionResult GetDetails(int id)
        //{
        //    var car = data.Cars.Find(id);

        //    if(car == null) return  NotFound();

        //    return Ok(car);
        //}

        //test
        //[HttpPost]
        //public IActionResult SaveCar(Car car)
        //{
        //    return Ok();
        //}

        [HttpGet]
        public CarQueryServiceModel All([FromQuery] AllCarsApiRequestModel query)
        {
            return this.carSevice.All(
                query.Brand,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                query.CarsPerPage);
        }
    }
}
