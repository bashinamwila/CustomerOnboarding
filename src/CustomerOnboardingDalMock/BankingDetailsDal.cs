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
    public class BankingDetailsDal : IBankingDetailsDal
    {
        public bool Exists(string tenantId)
        {
            var result = MockDb.BankingDetails
                .Any(r => r.TenantId == tenantId);
            return result;
        }

        public BankingDetailsDto Fetch(string tenantId)
        {
            var result = MockDb.BankingDetails
                .Where(r => r.TenantId == tenantId)
                .Select(r => new BankingDetailsDto
                {
                    TenantId = r.TenantId,
                    BankId = r.BankId,
                    BranchId = r.BranchId,
                    AccountNumber = r.AccountNumber,
                    LastChanged = r.LastChanged
                }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("BankingDetails");
            return result;
        }

        public void Insert(BankingDetailsDto data)
        {
            data.LastChanged=MockDb.GetTimeStamp();
            var newItem = new BankingDetailsEntity
            {
                TenantId = data.TenantId,
                BankId = data.BankId,
                BranchId = data.BranchId,
                AccountNumber = data.AccountNumber,
                LastChanged = data.LastChanged
            };
            MockDb.BankingDetails.Add(newItem);
        }

        public void Update(BankingDetailsDto data)
        {
            throw new NotImplementedException();
        }
    }
}
