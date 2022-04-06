using CarRenting.Models.Cars;

namespace CarRenting.Services.Cars
{
    public interface ICarSevice
    {
        CarQueryServiceModel All(
            string brand = null,
            string searchTerm = null,
            CarSorting sorting = CarSorting.DateCreated,
            int currentPage = 1,
            int carsPerPage = int.MaxValue,
            bool publicOnly = true);

        IEnumerable<LatestCarServiceModel> Latest();

        CarDetailsServiceModel Details(int carId);

        IEnumerable<CarServiceModel> ByUser(string userId);

        IEnumerable<string> AllCarBrands();

        IEnumerable<CarCategoryServiceModel> AllCarCategories();

        bool IsByDealer(int carId, int dealerId);

        bool CategoryExists(int categoriId);

        int Create(string brand,
                string model,
                string description,
                string imageUrl,
                int year,
                int categoryId,
                int dealerId);

        bool Edit(int carId,
                string brand,
                string model,
                string description,
                string imageUrl,
                int year,
                int categoryId,
                bool isPublic);

        void ChangeVisibility(int carId);
    }
}
