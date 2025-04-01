using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock
{
    public class StepDal : IStepDal
    {
        public List<StepDto> Fetch(string tenantId)
        {
            var list = (from r in MockDb.CreateAccounts
                        where r.TenantId == tenantId
                        select new StepDto
                        {
                            TenantId = r.TenantId,
                            Id = r.Id
                        }).ToList();

            list.AddRange(from r in MockDb.SendEmailNotifications
                          where r.TenantId == tenantId
                          select new StepDto
                          {
                              TenantId = r.TenantId,
                              Id = r.Id
                          });
            list.AddRange(from r in MockDb.EmailConfirmations
                          where r.TenantId == tenantId
                          select new StepDto
                          {
                              TenantId = r.TenantId,
                              Id = r.Id
                          });
            return list;
        }
    }
}
