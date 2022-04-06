using AutoMapper;
using CarRenting.Data.Models;
using CarRenting.Models.Cars;
using CarRenting.Services.Cars;

namespace CarRenting.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //standart mapping
            CreateMap<CarDetailsServiceModel, CarFormModel>();

            //mapping in lambda Select ....
            CreateMap<Car, LatestCarServiceModel>();

            //mapping with differents properties
            CreateMap<Car, CarDetailsServiceModel>()
                .ForMember(c => c.UserId, cfg => cfg.MapFrom(c => c.Dealer.UserId))
                .ForMember(c => c.CategoryName, cfg => cfg.MapFrom(c => c.Category.Name));
        }
    }
}
