using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock
{
    public class BranchDal : IBranchDal
    {
        public void Delete(string id, int branch)
        {
            throw new NotImplementedException();
        }

        public List<BranchDto> Fetch(string id)
        {
            var result=(from r in MockDb.Branches
                        join b in MockDb.Banks on r.BankId equals b.BankId
                        where r.BankId == id 
                        select new BranchDto
                        {
                            Id = r.Id,
                            Name = r.Name,
                            BranchCode = r.BranchCode,
                            LastChanged = r.LastChanged,
                            BankId = r.BankId
                        }).ToList();
            return result;
        }

        public void Insert(BranchDto data)
        {
            throw new NotImplementedException();
        }

        public void Update(BranchDto data)
        {
            throw new NotImplementedException();
        }
    }
}
