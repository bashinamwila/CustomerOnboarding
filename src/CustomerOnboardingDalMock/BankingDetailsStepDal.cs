using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using CustomerOnboarding.DalMock.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CustomerOnboarding.DalMock
{
    public class BankingDetailsStepDal :
        IBankingDetailsStepDal
    {
        public BankingDetailsStepDto Fetch(string tenantId, int id)
        {
            var result = (from r in MockDb.BankingDetailsSteps
                          join s in MockDb.Steps on r.Id equals s.Id
                          where r.TenantId == tenantId
                          && r.Id == id
                          select new BankingDetailsStepDto
                          {
                              TenantId = r.TenantId,
                              StepId = r.Id,
                              StepIndex = r.StepIndex,
                              Name = s.Name,
                              Type = s.Type,
                              IsCompleted=r.IsCompleted,
                              RuleSet = s.RuleSet,
                              LastChanged = r.LastChanged
                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("BankingDetailsStep");
            return result;
        }

        public void Insert(BankingDetailsStepDto dto)
        {
            dto.LastChanged = MockDb.GetTimeStamp();
            var newItem = new BankingDetailsStepEntity
            {
                TenantId = dto.TenantId,
                Id = dto.StepId,
                StepIndex = dto.StepIndex,
                IsCompleted = dto.IsCompleted,
                LastChanged = dto.LastChanged
            };
            MockDb.BankingDetailsSteps.Add(newItem);
        }

        public void Update(BankingDetailsStepDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
