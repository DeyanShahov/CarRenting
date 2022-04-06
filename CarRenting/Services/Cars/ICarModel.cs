namespace CarRenting.Services.Cars
{
    public interface ICarModel
    {
        string Brand { get; }

        string Model { get; }

        int Year { get; }
    }
}
