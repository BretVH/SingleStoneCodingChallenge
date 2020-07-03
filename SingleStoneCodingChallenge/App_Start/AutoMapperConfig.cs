using AutoMapper;
using SingleStoneCodingChallenge.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SingleStoneCodingChallenge.App_Start
{
    public static class AutoMapperConfig
    {
        public static IMapper RegisterMappings()
        {
           var config = new MapperConfiguration(cfg => {

               cfg.CreateMap<RawContact, ContactWithId>()
                   .ForMember(dest => dest.EMail, act => act.MapFrom(src => src.Email))
                   .ForPath(dest => dest.Address, act => act.MapFrom
                        (
                            src =>
                                new Address
                                {
                                    City = src.City,
                                    State = src.State,
                                    Street = src.Street,
                                    Zip = src.Zip
                                }))
                   .ForMember(dest => dest.Name, act => act.MapFrom
                   (
                       src =>
                       new Name
                       {
                           First = src.First,
                           Last = src.Last,
                           Middle = src.Middle
                       }))
                   .ForMember(dest => dest.Phone, act => act.MapFrom
                   (
                       src => new Phone[3]
                       {
                           new Phone()
                           {
                               Number = src.MobileNumber,
                               Type = src.MobileType
                           },
                           new Phone()
                           {
                               Number = src.HomeNumber,
                               Type = src.HomeType
                           },
                           new Phone()
                           {
                               Number = src.WorkNumber,
                               Type = src.WorkType
                           }
                       }))
                   .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id));
               cfg.CreateMap<RawContact, Contact>()
                   .ForMember(dest => dest.EMail, act => act.MapFrom(src => src.Email))
                   .ForMember(dest => dest.Address, act => act.MapFrom
                   (
                       src => new Address
                       {
                           City = src.City,
                           State = src.State,
                           Street = src.Street,
                           Zip = src.Zip
                       }))
                   .ForMember(dest => dest.Name, act => act.MapFrom
                   (
                       src => new Name
                       {
                           First = src.First,
                           Last = src.Last,
                           Middle = src.Middle
                       }))
                   .ForMember(dest => dest.Phone, act => act.MapFrom
                   (
                       src => new Phone[3]
                       {
                           new Phone()
                           {
                               Number = src.MobileNumber,
                               Type = src.MobileType
                           },
                           new Phone()
                           {
                               Number = src.HomeNumber,
                               Type = src.HomeType
                           },
                           new Phone()
                           {
                               Number = src.WorkNumber,
                               Type = src.WorkType
                           }
                       }));
               cfg.CreateMap<Contact, RawContact>()
                  .ForMember(dest => dest.Email, act => act.MapFrom(src => src.EMail))
                  .ForMember(dest => dest.First, act => act.MapFrom(src => src.Name.First))
                  .ForMember(dest => dest.Middle, act => act.MapFrom(src => src.Name.Middle))
                  .ForMember(dest => dest.Last, act => act.MapFrom(src => src.Name.Last))
                  .ForMember(dest => dest.Street, act => act.MapFrom(src => src.Address.Street))
                  .ForMember(dest => dest.City, act => act.MapFrom(src => src.Address.City))
                  .ForMember(dest => dest.State, act => act.MapFrom(src => src.Address.State))
                  .ForMember(dest => dest.Zip, act => act.MapFrom(src => src.Address.Zip))
                  .ForMember(dest => dest.MobileNumber, act => act.MapFrom(src => src.Phone.Where(c => c.Type.ToLower().Equals("mobile")).Select(c => c.Number).FirstOrDefault()))
                  .ForMember(dest => dest.MobileType, act => act.MapFrom(src => src.Phone.Where(c => c.Type.ToLower().Equals("mobile")).Select(c => c.Type).FirstOrDefault()))
                  .ForMember(dest => dest.HomeNumber, act => act.MapFrom(src => src.Phone.Where(c => c.Type.ToLower().Equals("home")).Select(c => c.Number).FirstOrDefault()))
                  .ForMember(dest => dest.HomeType, act => act.MapFrom(src => src.Phone.Where(c => c.Type.ToLower().Equals("home")).Select(c => c.Type).FirstOrDefault()))
                  .ForMember(dest => dest.WorkNumber, act => act.MapFrom(src => src.Phone.Where(c => c.Type.ToLower().Equals("work")).Select(c => c.Number).FirstOrDefault()))
                  .ForMember(dest => dest.WorkType, act => act.MapFrom(src => src.Phone.Where(c => c.Type.ToLower().Equals("work")).Select(c => c.Type).FirstOrDefault()));
           
                cfg.CreateMap<RawContact, ContactWithId>()
                   .ForMember(dest => dest.EMail, act => act.MapFrom(src => src.Email))
                   .ForMember(dest => dest.Address, act => act.MapFrom
                   (
                       src => new Address
                       {
                           City = src.City,
                           State = src.State,
                           Street = src.Street,
                           Zip = src.Zip
                       }))
                   .ForMember(dest => dest.Name, act => act.MapFrom
                   (
                       src => new Name
                       {
                           First = src.First,
                           Last = src.Last,
                           Middle = src.Middle
                       }))
                   .ForMember(dest => dest.Phone, act => act.MapFrom
                   (
                       src => new Phone[3]
                       {
                           new Phone()
                           {
                               Number = src.MobileNumber,
                               Type = src.MobileType
                           },
                           new Phone()
                           {
                               Number = src.HomeNumber,
                               Type = src.HomeType
                           },
                           new Phone()
                           {
                               Number = src.WorkNumber,
                               Type = src.WorkType
                           }
                       }))
                   .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id));
               cfg.CreateMap<ContactWithId, RawContact>()
                  .ForMember(dest => dest.Email, act => act.MapFrom(src => src.EMail))
                  .ForMember(dest => dest.First, act => act.MapFrom(src => src.Name.First))
                  .ForMember(dest => dest.Middle, act => act.MapFrom(src => src.Name.Middle))
                  .ForMember(dest => dest.Last, act => act.MapFrom(src => src.Name.Last))
                  .ForMember(dest => dest.Street, act => act.MapFrom(src => src.Address.Street))
                  .ForMember(dest => dest.City, act => act.MapFrom(src => src.Address.City))
                  .ForMember(dest => dest.State, act => act.MapFrom(src => src.Address.State))
                  .ForMember(dest => dest.Zip, act => act.MapFrom(src => src.Address.Zip))
                  .ForMember(dest => dest.MobileNumber, act => act.MapFrom(src => src.Phone.Where(c => c.Type.ToLower().Equals("mobile")).Select(c => c.Number).FirstOrDefault()))
                  .ForMember(dest => dest.MobileType, act => act.MapFrom(src => src.Phone.Where(c => c.Type.ToLower().Equals("mobile")).Select(c => c.Type).FirstOrDefault()))
                  .ForMember(dest => dest.HomeNumber, act => act.MapFrom(src => src.Phone.Where(c => c.Type.ToLower().Equals("home")).Select(c => c.Number).FirstOrDefault()))
                  .ForMember(dest => dest.HomeType, act => act.MapFrom(src => src.Phone.Where(c => c.Type.ToLower().Equals("home")).Select(c => c.Type).FirstOrDefault()))
                  .ForMember(dest => dest.WorkNumber, act => act.MapFrom(src => src.Phone.Where(c => c.Type.ToLower().Equals("work")).Select(c => c.Number).FirstOrDefault()))
                  .ForMember(dest => dest.WorkType, act => act.MapFrom(src => src.Phone.Where(c => c.Type.ToLower().Equals("work")).Select(c => c.Type).FirstOrDefault()))
                  .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id));
           });
            return config.CreateMapper();
        }
        
    }
}