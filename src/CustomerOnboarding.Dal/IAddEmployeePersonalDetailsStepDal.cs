using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IAddEmployeePersonalDetailsStepDal
    {
        public void Insert(AddEmployeePersonalDetailsStepDto dto);
        public void Update(AddEmployeePersonalDetailsStepDto dto);

        public AddEmployeePersonalDetailsStepDto Fetch(string tenantId, int id,int counter);
    }
}
