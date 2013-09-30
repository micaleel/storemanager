using AutoMapper;
using DialogueStore.Web.Models;
using DialogueStore.Web.ViewModels;
using DialogueStore.Web.Views.Stock;

namespace DialogueStore.Web.Infrastructure
{
    public class DialogueStoreProfile : Profile {
        protected override void Configure() {
            Mapper.CreateMap<CreateStockModel, Stock>();

            Mapper.CreateMap<RegisterModel, User>()
                  .ForMember(d => d.Password, o => o.MapFrom(s => s.Password));
            Mapper.CreateMap<User, RegisterModel>();
        }
    }
}