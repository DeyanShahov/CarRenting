using CarRenting.Data;
using CarRenting.Data.Models;
using CarRenting.Services.Dealers;
using CarRenting.Tests.Moq;
using Xunit;

namespace CarRenting.Tests.Services
{
    public class DealerServiceTest
    {
        Dealer dealer = new Dealer { Name = "Pesho", PhoneNumber = "088234455", UserId = "TestUserId" };

        [Fact]
        public void IsDealerShouldReturnTrueWhenUserIsDealer()
        {
            //Arange
            var dealerService = GetDealerService(dealer);
         
            //Act
            var result = dealerService.IsDealer(dealer.UserId);

            //Assert
            Assert.True(result);
        }

      

        [Fact]
        public void IsDealerShouldReturnFalseWhenUserIsNotDealer()
        {
            //Arange
            var dealerService = GetDealerService(dealer);

            //Act
            var result = dealerService.IsDealer("AnotherId");

            //Assert
            Assert.False(result);
        }


        private IDealerService GetDealerService(Dealer dealer)
        {            
            var data = DatabaseMock.Instance;

            data.Dealers.Add(dealer);

            data.SaveChanges();

            return new DealerService(data);
        }
    }
}
