using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock
{
    public class StepTypeDal : IStepTypeDal
    {
        public StepTypeDto Fetch(int id)
        {
            var result = (from r in MockDb.Steps
                          where r.Id == id
                          select new StepTypeDto
                          {
                              Id = r.Id,
                              Name = r.Name,
                              Type = r.Type,
                              FullTypeName = r.FullTypeName
                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("StepTypeInfo");
            return result;
        }
    }
}
