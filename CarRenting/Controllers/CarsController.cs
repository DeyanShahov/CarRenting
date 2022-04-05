using AutoMapper;
using CarRenting.Infrastructure;
using CarRenting.Models.Cars;
using CarRenting.Services.Cars;
using CarRenting.Services.Dealers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRenting.Controllers
{
    public class CarsController : Controller
    {
        private readonly IDealerService dealerSevice;

        private readonly ICarSevice carSevice;

        private readonly IMapper mapper;

        public CarsController(ICarSevice carSevice, IDealerService dealerSevice, IMapper mapper)
        {
            this.carSevice = carSevice;
            this.dealerSevice = dealerSevice;
            this.mapper = mapper;
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!dealerSevice.IsDealer(User.GetId()))
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            return View(new CarFormModel
            {
                Categories = this.carSevice.AllCarCategories()
            }); ;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(CarFormModel carModel)
        {
            var dealerId = this.dealerSevice.GetIdByUser(User.GetId());

            if (!dealerSevice.IsDealer(User.GetId()))
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }


            if (!carSevice.CategoryExists(carModel.CategoryId))
            {
                ModelState.AddModelError(nameof(carModel.CategoryId), "Category");
            }


            if (!ModelState.IsValid)
            {
                carModel.Categories = this.carSevice.AllCarCategories();

                return View(carModel);
            }

            carSevice.Create(carModel.Brand, carModel.Model, carModel.Description,
                carModel.ImageUrl, carModel.Year, carModel.CategoryId, dealerId);

            //Add TempData
            TempData[WebConstants.GlobalMessageKey] = "You car was added";

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

    
        public IActionResult Edit(int id)
        {
            var userId = User.GetId();

            // proverka dali smeDilari vaobshte ili ne si admin
            if (!dealerSevice.IsDealer(userId) && !User.IsAdmin())
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            var car = carSevice.Details(id);

            // proverka dali dadenata kola e moq za da moga da q promenqm ili ne si admin
            if (car.UserId != userId && !User.IsAdmin())
            {
                return Unauthorized();
            }

            //Mapping using AutoMapper
            var carForm = mapper.Map<CarFormModel>(car);

            carForm.Categories = carSevice.AllCarCategories();

            return View(carForm);

            //Standart maping objeckts
            //return View(new CarFormModel
            //{
            //    Brand = car.Brand,
            //    Model = car.Model,
            //    Description = car.Description,
            //    ImageUrl = car.ImageUrl,
            //    Year = car.Year,
            //    CategoryId = car.CategoryId,        
            //    Categories = this.carSevice.AllCarCategories()
            //});
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, CarFormModel carModel)
        {
            var dealerId = this.dealerSevice.GetIdByUser(User.GetId());

            //proverka dali e dilar potrebitelq ili ne e admin
            if (dealerId == 0 && !User.IsAdmin())
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            //proverka ima li takava kategoriq
            if (!carSevice.CategoryExists(carModel.CategoryId))
            {
                ModelState.AddModelError(nameof(carModel.CategoryId), "Category");
            }

            //proverka na popalnenite danni
            if (!ModelState.IsValid)
            {
                carModel.Categories = this.carSevice.AllCarCategories();

                return View(carModel);
            }

            //proverka dali dilara ima pravo da promenq tochno tazi kola, tq ot neegovite koli li e ?
            if (!carSevice.IsByDealer(id, dealerId) && !User.IsAdmin())
            {
                return BadRequest();
            }

            carSevice.Edit(id, carModel.Brand, carModel.Model, carModel.Description,
                carModel.ImageUrl, carModel.Year, carModel.CategoryId);


            //Add TempData
            TempData[WebConstants.GlobalMessageKey] = "You car was edited successfully!";


            return RedirectToAction(nameof(All));
        }

     

    }
}
