using CarRenting.Data;
using CarRenting.Data.Models;
using CarRenting.Models.Cars;

namespace CarRenting.Services.Cars
{
    public class CarService : ICarSevice
    {
        private readonly CarRentingDbContext data;

        public CarService(CarRentingDbContext data)
        {
            this.data = data;
        }

        public CarQueryServiceModel All(string brand, string searchTerm, CarSorting sorting, int currentPage,
            int carsPerPage)
        {
            var carsQuery = data.Cars.AsQueryable();

            if (!string.IsNullOrWhiteSpace(brand))
            {
                carsQuery = carsQuery.Where(c => c.Brand == brand);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                carsQuery = carsQuery.Where(c =>
                    c.Brand.ToLower().Contains(searchTerm.ToLower())
                    || c.Model.ToLower().Contains(searchTerm.ToLower())
                    || c.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            carsQuery = sorting switch
            {
                CarSorting.DateCreated => carsQuery.OrderByDescending(c => c.Id),
                CarSorting.Year => carsQuery.OrderByDescending(c => c.Year),
                CarSorting.BrandAndModel => carsQuery.OrderBy(c => c.Brand).ThenBy(c => c.Model),
                _ => carsQuery.OrderByDescending(c => c.Id)
            };

            var totalCars = carsQuery.Count();

            var cars = GetCars(carsQuery
                .Skip((currentPage - 1) * carsPerPage)
                .Take(carsPerPage));
                

            return new CarQueryServiceModel
            {
                TotalCars = totalCars,
                CurrentPage = currentPage,
                CarsPerPage = carsPerPage,
                Cars = cars
            };
        }

        public IEnumerable<string> AllCarBrands()
        {
            return data
                .Cars
                .Select(c => c.Brand)
                .Distinct()
                .OrderBy(br => br)
                .ToList();
        }

        public IEnumerable<CarServiceModel> ByUser(string userId)
        {
            return GetCars(this.data
                .Cars
                .Where(c => c.Dealer.UserId == userId));
        }

        private static IEnumerable<CarServiceModel> GetCars(IQueryable<Car> carQery)
        {
            return carQery
                .Select(c => new CarServiceModel
                {
                    Id = c.Id,
                    Brand = c.Brand,
                    Model = c.Model,
                    Year = c.Year,
                    ImageUrl = c.ImageUrl,
                    Category = c.Category.Name
                })
                .ToList();
        }

        public IEnumerable<CarCategoryServiceModel> AllCarCategories()
        {
            return this.data
                    .Categories
                    .Select(c => new CarCategoryServiceModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                    })
                    .ToList();
        }
    }
}
