using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IBranchDal
    {
        void Insert(BranchDto data);
        void Update(BranchDto data);
        void Delete(string id, int branch);
        List<BranchDto> Fetch(string id);

    }
}
