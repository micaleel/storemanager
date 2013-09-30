using AutoMapper;
using DialogueStore.Models;
using DialogueStore.ViewModels;
using DialogueStore.Views.Stock;

namespace DialogueStore.Infrastructure
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