using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IConfirmEmailStepDal
    {
        public void Insert(ConfirmEmailStepDto data);
       public ConfirmEmailStepDto Fetch(string tenantId,int stepId);
    }
}
