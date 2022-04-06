using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarRenting.Data;
using CarRenting.Data.Models;
using CarRenting.Models.Cars;

namespace CarRenting.Services.Cars
{
    public class CarService : ICarSevice
    {
        private readonly CarRentingDbContext data;
        private readonly IMapper mapper;

        public CarService(CarRentingDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public CarQueryServiceModel All(string brand, string searchTerm, CarSorting sorting, int currentPage,
            int carsPerPage)
        {
            var carsQuery = data.Cars
                .Where(c => c.IsPublic);

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
                    CategoryName = c.Category.Name
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

        public CarDetailsServiceModel Details(int id)
        {
            return data
                .Cars
                .Where(c => c.Id == id)
                .ProjectTo<CarDetailsServiceModel>(mapper.ConfigurationProvider)
                //.Select(c => new CarDetailsServiceModel
                //{
                //    Id = c.Id,
                //    Brand = c.Brand,
                //    Model = c.Model,
                //    Year = c.Year,
                //    ImageUrl = c.ImageUrl,
                //    CategoryId = c.Category.Id,
                //    CategoryName = c.Category.Name,
                //    Description = c.Description,
                //    DealerId = c.DealerId,
                //    DealerName = c.Dealer.Name,
                //    UserId = c.Dealer.UserId

                //})
                .FirstOrDefault();
        }

        public bool CategoryExists(int categoriId)
        {
            return data.Categories.Any(c => c.Id == categoriId);
        }

        public int Create(string brand, string model, string description, string imageUrl, int year, int categoryId, int dealerId)
        {
            var car = new Car
            {
                Brand = brand,
                Model = model,
                Description = description,
                ImageUrl = imageUrl,
                Year = year,
                CategoryId = categoryId,
                DealerId = dealerId,
                IsPublic = false
            };

            data.Cars.Add(car);

            data.SaveChanges();

            return car.Id;
        }

        public bool Edit(int id, string brand, string model, string description, string imageUrl, int year, int categoryId)
        {
            //namiram kolata v bazata po podadenoto id
            var car = data.Cars.FirstOrDefault(c => c.Id == id);

            if (car == null)
            {
                return false;
            }


            //zapomnqm novite parametri po kolata 
            car.Brand = brand;
            car.Model = model;
            car.Description = description;
            car.ImageUrl = imageUrl;
            car.Year = year;
            car.CategoryId = categoryId;
            car.IsPublic = false;

            //zapametqvam promenite
            data.SaveChanges();

            return true;
        }

        public bool IsByDealer(int carId, int dealerId)
        {
           return data
                .Cars
                .Any(c => c.Id == carId && c.DealerId == dealerId);
        }

        public IEnumerable<LatestCarServiceModel> Latest()
        {
           return this.data
                .Cars
                .Where(c => c.IsPublic)
                .OrderByDescending(c => c.Id)
                .ProjectTo<LatestCarServiceModel>(mapper.ConfigurationProvider)
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
        }
    }
}
