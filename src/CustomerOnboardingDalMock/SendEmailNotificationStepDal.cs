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
    public class SendEmailNotificationStepDal : ISendEmailNotificationStepDal
    {
        public SendEmailNotificationStepDto Fetch(string tenantId, int id)
        {
            var result = (from r in MockDb.SendEmailNotifications
                          join s in MockDb.Steps on r.Id equals s.Id
                          where r.TenantId == tenantId && r.Id == id
                          select new SendEmailNotificationStepDto
                          {
                              Id = r.Id,
                              TenantId = r.TenantId,
                              LastChanged = r.LastChanged,
                              Name = s.Name,
                              Type = s.Type,
                              StepIndex = r.StepIndex,
                              IsCompleted = r.IsCompleted
                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("SendEmailNoticationStep");
            return result;
        }

        public void Insert(SendEmailNotificationStepDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new SendEmailNotificationStepEntity
            {
                TenantId= data.TenantId,
                Id = data.Id,
                StepIndex = data.StepIndex,
                IsCompleted = data.IsCompleted,
                LastChanged = data.LastChanged
            };
            MockDb.SendEmailNotifications.Add(newItem);
        }
    }
}
