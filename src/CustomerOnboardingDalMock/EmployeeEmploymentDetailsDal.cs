using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using CustomerOnboarding.DalMock.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock
{
    public class EmployeeEmploymentDetailsDal : IEmployeeEmploymentDetailsDal
    {
        public EmployeeEmploymentDetailsDto Fetch(string tenantId, string employeeId)
        {
            var result = MockDb.EmployeeEmploymentDetails.Where(r => r.TenantId == tenantId && r.EmployeeId == employeeId)
                        .Select(r => new EmployeeEmploymentDetailsDto
                        {
                            EmployeeId = r.EmployeeId,
                            FirstName = r.FirstName,
                            LastName = r.LastName,
                            StatusId = r.StatusId,
                            PayType = r.PayType,
                            EmploymentType = r.EmploymentType,
                            HireDate = r.HireDate,
                            Group = r.Group,
                            DeptId = r.DeptId,
                            JobTitle = r.JobTitle,
                            ReportsTo = r.ReportsTo,
                            ReportsToEmployeeId = r.ReportsToEmployeeId
                        }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("EmployeeEmploymentDetails");
            return result;

        }

        public void Insert(EmployeeEmploymentDetailsDto dto)
        {
            dto.LastChanged = MockDb.GetTimeStamp();
            var newItem = new EmployeeEmploymentDetailsEntity
            {
                TenantId = dto.TenantId,
                EmployeeId = dto.EmployeeId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                StatusId = dto.StatusId,
                PayType = dto.PayType,
                EmploymentType = dto.EmploymentType,
                HireDate = dto.HireDate,
                Group = dto.Group,
                DeptId = dto.DeptId,
                JobTitle = dto.JobTitle,
                ReportsTo = dto.ReportsTo,
                ReportsToEmployeeId = dto.ReportsToEmployeeId
            };
            MockDb.EmployeeEmploymentDetails.Add(newItem);
        }

        public void Update(EmployeeEmploymentDetailsDto dto)
        {
            var result = MockDb.EmployeeEmploymentDetails.
                         Where(r => r.TenantId == dto.TenantId &&
                         r.EmployeeId == dto.EmployeeId)
                        .Select(r => r).FirstOrDefault();

            if (result is null)
                throw new DataNotFoundException("EmployeeEmploymentDetails");
            if (!dto.LastChanged.Matches(dto.LastChanged))
                throw new ConcurrencyException("EmployeeEmploymentDetails");
            dto.LastChanged = MockDb.GetTimeStamp();
            result.FirstName = dto.FirstName;
            result.LastName = dto.LastName;
            result.StatusId = dto.StatusId;
            result.PayType = dto.PayType;
            result.EmploymentType = dto.EmploymentType;
            result.HireDate = dto.HireDate;
            result.Group = dto.Group;
            result.DeptId = dto.DeptId;
            result.JobTitle = dto.JobTitle;
            result.ReportsTo = dto.ReportsTo;
            result.ReportsToEmployeeId = dto.ReportsToEmployeeId;
            result.LastChanged = dto.LastChanged;

        }
    }
}
