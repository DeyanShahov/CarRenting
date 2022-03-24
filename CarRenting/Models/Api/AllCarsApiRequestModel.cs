using CarRenting.Models.Cars;

namespace CarRenting.Models.Api
{
    public class AllCarsApiRequestModel
    {
        public string Brand { get; init; }

        public string SearchTerm { get; init; }

        public CarSorting Sorting { get; init; }

        public int CarsPerPage { get; init; } = 10;

        public int CurrentPage { get; init; } = 1;

        public int TotalCars { get; set; }
     
    }
}
