using CarRenting.Services.Cars;
using System.ComponentModel.DataAnnotations;

namespace CarRenting.Models.Cars
{
    public class AllCarsQueryModel
    {
        public const int CarsPerPage = 2;

        public int CurrentPage { get; init; } = 1;

        public int TotalCars { get; set; }

        public string Brand { get; init; }

        public IEnumerable<string> Brands { get; set; }

        [Display(Name = "Search")]
        public string SearchTerm { get; init; }

        public CarSorting Sorting { get; init; }


        public IEnumerable<CarServiceModel> Cars { get; set; }
    }
}
