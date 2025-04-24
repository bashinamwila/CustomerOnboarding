using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface ICompensationComponentsStepDal
    {
        public void Insert(CompensationComponentsStepDto dto);
        public void Update(CompensationComponentsStepDto dto);
        public CompensationComponentsStepDto Fetch(string tenantId,int id);
    }
}
