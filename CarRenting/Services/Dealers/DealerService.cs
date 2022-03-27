using CarRenting.Data;

namespace CarRenting.Services.Dealers
{
    public class DealerService : IDealerService
    {
        private readonly CarRentingDbContext data;

        public DealerService(CarRentingDbContext data)
        {
            this.data = data;
        }

        public bool IsDealer(string userId) => data.Dealers.All(d => d.UserId == userId);
    }
}
