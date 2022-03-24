using CarRenting.Models.Cars;

namespace CarRenting.Services.Cars
{
    public interface ICarSevice
    {
        CarQueryServiceModel All(string brand, string searchTerm, CarSorting sorting, int currentPage, int carsPerPage);

        IEnumerable<string> AllCarBrands();
    }
}
