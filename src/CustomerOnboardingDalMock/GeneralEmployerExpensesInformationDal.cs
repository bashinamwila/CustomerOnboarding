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
    public class GeneralEmployerExpensesInformationDal :
        IGeneralEmployerExpensesInformationDal
    {
        public GeneralEmployerExpensesInformationDto Fetch(string tenantId, int id)
        {
            var result = (from r in MockDb.GeneralEmployerExpensesInformationSteps
                          join s in MockDb.Steps on r.Id equals s.Id
                          where r.TenantId == tenantId
                          && r.Id == id
                          select new GeneralEmployerExpensesInformationDto
                          {
                              TenantId = r.TenantId,
                              StepId = r.Id,
                              StepIndex = r.StepIndex,
                              Name = s.Name,
                              Type = s.Type,
                              IsCompleted = r.IsCompleted,
                              LastChanged = r.LastChanged
                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("GeneralEmployerExpensesInformationStep");
            return result;
        }

        public void Insert(GeneralEmployerExpensesInformationDto dto)
        {
            dto.LastChanged = MockDb.GetTimeStamp();
            var newItem = new GeneralEmployerExpensesInformationEntity
            {
                TenantId = dto.TenantId,
                Id = dto.StepId,
                StepIndex = dto.StepIndex,
                IsCompleted = dto.IsCompleted,
                LastChanged = dto.LastChanged
            };
            MockDb.GeneralEmployerExpensesInformationSteps.Add(newItem);
        }

        public void Update(GeneralEmployerExpensesInformationDto dto)
        {
            var result = (from r in MockDb.GeneralEmployerExpensesInformationSteps
                          where r.TenantId == dto.TenantId
                          select r).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("GeneralEmployerExpensesInformationStep");
            if (!result.LastChanged.Matches(dto.LastChanged))
                throw new ConcurrencyException("GeneralEmployerExpensesInformationStep");
            dto.LastChanged = MockDb.GetTimeStamp();
            result.IsCompleted = dto.IsCompleted;
            result.LastChanged = dto.LastChanged;
        }
    }
}

