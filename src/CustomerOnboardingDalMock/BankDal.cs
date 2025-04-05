using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock
{
    public class BankDal : IBankDal
    {
        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public BankDto Fetch(string id)
        {
            var result = (from r in MockDb.Banks
                          where r.BankId == id
                          select new BankDto
                          {
                              Id = r.BankId,
                              Name = r.Name,
                              SwiftCode = r.SwiftCode,
                              LastChanged = r.LastChanged
                          }).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("Bank");
            return result;

        }

        public List<BankDto> Fetch()
        {
            var result = (from r in MockDb.Banks
                          select new BankDto
                          {
                              Id = r.BankId,
                              Name = r.Name,
                              SwiftCode = r.SwiftCode,
                              LastChanged = r.LastChanged
                          }).ToList();
            return result;

        }

        public void Insert(BankDto data)
        {
            throw new NotImplementedException();
        }

        public void Update(BankDto data)
        {
            throw new NotImplementedException();
        }
    }
}
