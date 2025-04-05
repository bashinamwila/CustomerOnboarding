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
    public class OrganisationProfileStepDal :
        IOrganisationProfileStepDal
    {
        public OrganisationProfileStepDto Fetch(string tenantId, int id)
        {
            var result = (from r in MockDb.OrganisationProfileSteps
                          join s in MockDb.Steps on r.Id equals s.Id
                          where r.TenantId == tenantId
                          && r.Id == id
                          select new OrganisationProfileStepDto
                          {
                              TenantId = r.TenantId,
                              StepId = r.Id,
                              StepIndex = r.StepIndex,
                              Name = s.Name,
                              Type = s.Type,
                              RuleSet = s.RuleSet,
                              IsCompleted=r.IsCompleted,
                              LastChanged = r.LastChanged
                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("OrganisationProfileStep");
            return result;
        }

        public void Insert(OrganisationProfileStepDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new OrganisationProfileStepEntity
            {
                TenantId = data.TenantId,
                Id = data.StepId,
                StepIndex = data.StepIndex,
                IsCompleted = data.IsCompleted,
                LastChanged = data.LastChanged
            };
            MockDb.OrganisationProfileSteps.Add(newItem);
        }

        public void Update(OrganisationProfileStepDto data)
        {
            throw new NotImplementedException();
        }
    }
}
