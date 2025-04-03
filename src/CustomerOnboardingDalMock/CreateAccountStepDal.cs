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
    public class CreateAccountStepDal : ICreateAccountStepDal
    {
        public CreateAccountStepDto Fetch(string tenantId, int id)
        {
            var result = (from r in MockDb.CreateAccounts
                          join s in MockDb.Steps on r.Id equals s.Id
                          where r.TenantId == tenantId
                          && r.Id == id
                          select new CreateAccountStepDto
                          {
                              TenantId = r.TenantId,
                              StepId = r.Id,
                              StepIndex=r.StepIndex,
                              Name= s.Name,
                              Type = s.Type,
                              RuleSet=s.RuleSet,
                              LastChanged = r.LastChanged
                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("CreateAccountStep");
            return result;
        }

        public void Insert(CreateAccountStepDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new CreateAccountStepEntity
            {
                TenantId = data.TenantId,
                Id = data.StepId,
                StepIndex = data.StepIndex,
                LastChanged = data.LastChanged
            };
            MockDb.CreateAccounts.Add(newItem);
        }

        public void Update(CreateAccountStepDto data)
        {
           // throw new NotImplementedException();
        }
    }
}
