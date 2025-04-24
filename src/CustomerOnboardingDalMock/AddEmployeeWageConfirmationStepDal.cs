using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using CustomerOnboarding.DalMock.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CustomerOnboarding.DalMock
{
    public class AddEmployeeWageConfirmationStepDal : IAddEmployeeWageConfirmationStepDal
    {
        public AddEmployeeWageConfirmationStepDto Fetch(string tenantId, int id,string employeeId,string wageId)
        {
            var result = (from r in MockDb.AddEmployeeWageConfirmationSteps
                          join s in
                          MockDb.Steps on r.Id equals s.Id
                          where r.TenantId == tenantId && r.Id == id
                          && r.EmployeeId==employeeId && r.WageId==wageId
                          select new AddEmployeeWageConfirmationStepDto
                          {
                              StepId = r.Id,
                              WageId = r.WageId,
                              EmployeeId = r.EmployeeId,
                              Name = s.Name,
                              Type = s.Type,
                              StepIndex = r.StepIndex,
                              IsCompleted = r.IsCompleted,
                              ActionTaken = r.ActionTaken,
                              LastChanged = r.LastChanged
                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("AddEmployeeWageConfirmationStep");
            return result;
        }

        public void Insert(AddEmployeeWageConfirmationStepDto dto)
        {
            dto.LastChanged = MockDb.GetTimeStamp();
            var newItem = new AddEmployeeWageConfirmationStepEntity
            {
                TenantId=dto.TenantId,
                EmployeeId=dto.EmployeeId,
                Id=dto.StepId,
                StepIndex=dto.StepIndex,
                IsCompleted=dto.IsCompleted,
                WageId=dto.WageId,
                ActionTaken=dto.ActionTaken,
                LastChanged=dto.LastChanged
            };
            MockDb.AddEmployeeWageConfirmationSteps.Add(newItem);
        }

        public void Update(AddEmployeeWageConfirmationStepDto dto)
        {
            var result = MockDb.AddEmployeeWageConfirmationSteps.Where(r => r.TenantId == dto.TenantId &&
                                                        r.Id == dto.StepId && r.EmployeeId==dto.EmployeeId
                                                        && r.WageId==dto.WageId)
                                                    .Select(r => r).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("AddEmployeeWageConfirmationStep");
            if (!result.LastChanged.Matches(dto.LastChanged))
                throw new DataNotFoundException("AddEmployeeWageConfirmationStep");
            dto.LastChanged = MockDb.GetTimeStamp();
            result.ActionTaken = dto.ActionTaken;
            result.IsCompleted = dto.IsCompleted;
            result.LastChanged = dto.LastChanged;


        }
    }
}
