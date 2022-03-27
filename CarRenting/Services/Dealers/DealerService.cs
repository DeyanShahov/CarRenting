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

        public int GetIdByUser(string userId)
        {
            return data
                .Dealers
                .Where(d => d.UserId == userId)
                .Select(d => d.Id)
                .FirstOrDefault();
        }

        public bool IsDealer(string userId) => data.Dealers.Any(d => d.UserId == userId);
    }
}
