using AutoMapper;
using PM.Domain.Cities;
using PM.Domain.People;
using PM.Infrastructure.EF.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PM.Infrastructure.Mapper
{
    public class EFProfile : Profile
    {
        public EFProfile()
        {
            CreateMap<Person, PersonEntity>().ForMember(
                dest => dest.City,
                opt => opt.Ignore());

            CreateMap<PersonEntity, RelatedPerson>()
                     .ForMember(
                         dest => dest.PhoneNumber,
                         opt => opt.MapFrom(src => new PhoneNumber(src.PhoneNumber, src.PhoneNumberType)));

            CreateMap<PersonEntity, Person>()
                        .ForMember(
                            dest => dest.PhoneNumber,
                            opt => opt.MapFrom(src => new PhoneNumber(src.PhoneNumber, src.PhoneNumberType)))
                         .ForMember(
                            dest => dest.City,
                            opt => opt.MapFrom(src => src.City == null ? null : src.City.Name))
                         .ForMember(
                            dest => dest.RelatedPeople,
                            opt => opt.MapFrom(src => src.Relations.Select(s => 
                                new RelatedPerson {ID = s.RelatedPersonID, RelationType = s.RelationType }
                            )));


            CreateMap<City, CityEntity>();
            CreateMap<CityEntity, City>();
        }

        public class RelationPersonResolver : IValueResolver<PersonEntity, Person, IReadOnlyCollection<RelatedPerson>>
        {
            public IReadOnlyCollection<RelatedPerson> Resolve(PersonEntity source, Person dest, IReadOnlyCollection<RelatedPerson> destMember, ResolutionContext context)
            {
                var res = source
                    .Relations
                    .Select(s => {
                        var rp = context.Mapper.Map<RelatedPerson>(s.RelatedPerson);
                        rp.RelationType = s.RelationType;
                        return rp; 
                    })
                    .ToList();
                return new ReadOnlyCollection<RelatedPerson>(res);
            }
        }
    }
}
