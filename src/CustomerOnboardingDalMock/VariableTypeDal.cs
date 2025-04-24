using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock
{
    public class VariableTypeDal : IVariableTypeDal
    {
        public List<VariableTypeDto> Fetch()
        {
            var result = (from r in MockDb.VariableTypes
                          select new VariableTypeDto
                          {
                              Id = r.Id,
                              Pattern = r.Pattern,
                              TypeName = r.TypeName,
                              LastChanged = r.LastChanged
                          }).ToList();
            return result;
        }

        public VariableTypeDto Fetch(int id)
        {
            var result = (from r in MockDb.VariableTypes
                          where r.Id == id
                          select new VariableTypeDto
                          {
                              Id = r.Id,
                              Pattern = r.Pattern,
                              TypeName = r.TypeName,
                              LastChanged = r.LastChanged
                          }).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("VariableType");
            return result;
        }
    }
}
