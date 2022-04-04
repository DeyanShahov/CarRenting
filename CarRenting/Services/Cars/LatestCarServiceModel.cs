namespace CarRenting.Services.Cars
{
    public class LatestCarServiceModel
    {
        public int Id { get; init; }

        public string Brand { get; init; }

        public string Model { get; init; }

        public string ImageUrl { get; init; }

        public string Description { get; set; }

        public int Year { get; init; }
    }
}
