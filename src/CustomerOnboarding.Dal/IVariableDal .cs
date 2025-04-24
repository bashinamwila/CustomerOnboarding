using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IVariableDal
    {
        List<VariableDto> Fetch(string tenantId,string id);
        VariableDto Fetch(string tenantId,int id, string itemId);
        void Insert(VariableDto data);
        void Update(VariableDto data);
        Task DeleteAsync(string itemId, int id);
    }
}
