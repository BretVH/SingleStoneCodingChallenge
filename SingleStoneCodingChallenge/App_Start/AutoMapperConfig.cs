using AutoMapper;
using SingleStoneCodingChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SingleStoneCodingChallenge.App_Start
{
    public static class AutoMapperConfig
    {
        public static IMapper RegisterMappings()
        {
           var config = new MapperConfiguration(cfg => {
               cfg.CreateMap<NameModel, Name>().ReverseMap();
               cfg.CreateMap<AddressModel, Address>().ReverseMap();
               cfg.CreateMap<PhoneModel, Phone>().ReverseMap();
               cfg.CreateMap<Contact, ContactModel>()
                   .ForMember(dest => dest.EMail, act => act.MapFrom(src => src.EMail))
                   .ForMember(dest => dest.Address, act => act.MapFrom(src => src.Address))
                   .ForMember(dest => dest.Name, act => act.MapFrom(src => new Name{ EMail = src.EMail, First = src.Name.First, Last = src.Name.Last, Middle = src.Name.Middle }))
                   .ForMember(dest => dest.Phone, act => act.MapFrom(src => src.Phone))
                   .ReverseMap();
         });
            return config.CreateMapper();
        }
        
    }
}