using AutoMapper;
using StoreManager.Models;
using StoreManager.ViewModels;
using StoreManager.Views.Stock;

namespace StoreManager.Infrastructure
{
    public class StoreManagerProfile : Profile {
        protected override void Configure() {
            Mapper.CreateMap<CreateStockModel, Stock>();

            Mapper.CreateMap<RegisterModel, User>()
                  .ForMember(d => d.Password, o => o.MapFrom(s => s.Password));
            Mapper.CreateMap<User, RegisterModel>();
        }
    }
}