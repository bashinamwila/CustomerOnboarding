using CustomerOnboarding.Dal.Dtos;
using CustomerOnboarding.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock
{
    public class CountryDal : ICountryDal
    {
        public List<CountryDto> Fetch()
        {
            var countries = (from r in MockDb.Countries
                             select new CountryDto
                             {
                                 Id = r.Id,
                                 Name = r.Name,
                             }).ToList();
            return countries;
        }
    }
}
