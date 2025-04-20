using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock
{
    public class StatusTypeDal : IStatusTypeDal
    {
        public TypeDto Fetch(int id)
        {
            var result = (from r in MockDb.StatusTypes
                          where r.Id == id
                          select new TypeDto
                          {
                              Id = r.Id,
                              Name = r.Name,
                              FriendlyName = r.FriendlyName,
                              TypeName = r.FullTypeName,
                              ComponentTypeName = r.ComponentTypeName
                          }).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("StatusType");
            return result;
        }

        public List<TypeDto> Fetch()
        {
            throw new NotImplementedException();
        }
    }
}
