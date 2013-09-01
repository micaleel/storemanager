using AutoMapper;
using StoreManager.Models;
using StoreManager.ViewModels;

namespace StoreManager.Controllers
{
    public class StoreManagerWebProfile : Profile {
        public override string ProfileName {
            get { return "AutoMapperProfile"; }
        }

        protected override void Configure() {
            Mapper.CreateMap<RegisterModel, User>()
                  .ForMember(d => d.Password, o => o.MapFrom(s => s.Password));
            Mapper.CreateMap<User, RegisterModel>();
        }
    }
}