using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IGeneralEmployerExpensesInformationDal
    {
        public void Insert(GeneralEmployerExpensesInformationDto dto);
        public void Update(GeneralEmployerExpensesInformationDto dto);

        public GeneralEmployerExpensesInformationDto Fetch(string tenantId, int id);
    }
}
