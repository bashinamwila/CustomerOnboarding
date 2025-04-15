using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface ICompensationComponentsStepStepsDal
    {
        public List<StepDto> Fetch(string tenantId);
    }
}
