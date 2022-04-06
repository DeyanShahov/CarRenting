using CarRenting.Services.Cars;

namespace CarRenting.Infrastructure
{
    public static class ModelExtensions
    {
        public static string ToFriendlyUrl(this ICarModel car)
            => car.Brand + "-" + car.Model + "-" + car.Year;
    }
}
