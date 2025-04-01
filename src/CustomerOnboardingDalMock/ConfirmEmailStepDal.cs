using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using CustomerOnboarding.DalMock.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock
{
    public class ConfirmEmailStepDal : IConfirmEmailStepDal
    {
        public ConfirmEmailStepDto Fetch(string tenantId, int stepId)
        {
            var result = (from r in MockDb.EmailConfirmations
                          join s in MockDb.Steps on r.Id equals s.Id
                          where r.TenantId == tenantId && r.Id == stepId
                          select new ConfirmEmailStepDto
                          {
                              TenantId=r.TenantId,
                              Name = s.Name,
                              Type = s.Type,
                              RuleSet = s.RuleSet,
                              StepIndex = r.StepIndex,
                              LastChanged = r.LastChanged

                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("ConfirmEmailStep");
            return result;
        }

        public void Insert(ConfirmEmailStepDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem=new ConfirmEmailStepEntity
            {
                TenantId = data.TenantId,
                Id = data.StepId,
                StepIndex = data.StepIndex,
                LastChanged = data.LastChanged
            };
            MockDb.EmailConfirmations.Add(newItem);
        }

        
    }
}
