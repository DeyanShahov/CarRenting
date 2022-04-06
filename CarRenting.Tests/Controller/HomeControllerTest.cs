using AutoMapper;
using CarRenting.Controllers;
using CarRenting.Data.Models;
using CarRenting.Models.Home;
using CarRenting.Services.Cars;
using CarRenting.Services.Statistics;
using CarRenting.Tests.Moq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq;
using Xunit;

namespace CarRenting.Tests.Controller
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnViewWithCorrectModel()
        {
            ////Arange
            //var data = DatabaseMock.Instance;
            //var mapper = MapperMock.Instance;

            ////var cars = Enumerable.Range(0, 10).Select(i => new Mock<Car>());
            //var cars = Enumerable.Range(0, 10).Select(i => new Car { Brand = "fff", Description ="fffff", ImageUrl="ffff", Model="gggg"});

            //data.Cars.AddRange(cars);
            //data.Users.Add(new User { Email="ffff", FullName="ffffff"});

            //data.SaveChanges();

            //var carService = new CarService(data, mapper);
            //var statisticsService = new StatisticsService(data);

            //var homeController = new HomeController(statisticsService, carService);

            ////Act
            //var result = homeController.Index();

            ////Assert
            //Assert.NotNull(result);

            //var viewResult = Assert.IsType<ViewResult>(result);

            //var model = viewResult.Model;

            //var indexViewModel = Assert.IsType<IndexViewModel>(model);

            //Assert.Equal(3, indexViewModel.Cars.Count);   
            //Assert.Equal(10, indexViewModel.TotalCars);   
            //Assert.Equal(1, indexViewModel.TotalUsers);
        }


        [Fact]
        public void ErrorShouldReturnView()
        {
            //Arange
            var homeController = new HomeController(null, null);

            //Act
            var result = homeController.Error();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}
