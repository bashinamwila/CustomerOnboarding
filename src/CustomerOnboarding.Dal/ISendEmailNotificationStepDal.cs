using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface ISendEmailNotificationStepDal
    {
        public Task InsertAsync(SendEmailNotificationStepDto data);
    }
}
