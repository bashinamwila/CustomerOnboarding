using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock
{
    public class EmployeeDataInputMethodTypeDal : IEmployeeDataInputMethodTypeDal
    {
        public TypeDto Fetch(int id)
        {
            var result = (from r in MockDb.EmployeeDataInputMethodTypes
                          where r.Id == id
                          select new TypeDto
                          {
                              Id = r.Id,
                              TypeName = r.FullTypeName,
                              Name = r.Name,
                              FriendlyName = r.FriendlyName,
                              ComponentTypeName = r.ComponentTypeName
                          }).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("EmployeeDataInputMethodType");
            return result;
        }

        public List<TypeDto> Fetch()
        {
            throw new NotImplementedException();
        }
    }
}
