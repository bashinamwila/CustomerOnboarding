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
    public class VariableDal : IVariableDal
    {
        public Task DeleteAsync(string itemId, int id)
        {
            throw new NotImplementedException();
        }

        public List<VariableDto> Fetch(string tenantId, string id)
        {
            var result = (from r in MockDb.Variables
                          where r.ItemId == id && r.TenantId==tenantId
                          select new VariableDto
                          {
                              Id = r.Id,
                              ItemId = r.ItemId,
                              Token = r.Token,
                              Value = r.Value,
                              LastChanged = r.LastChanged
                          }).ToList();
            return result;
        }

        public VariableDto Fetch(string tenantId, int id, string itemId)
        {
            var result = (from r in MockDb.Variables
                         where r.Id == id && r.ItemId == itemId
                         && r.TenantId==tenantId
                         select new VariableDto
                         {
                             Id = r.Id,
                             Token = r.Token,
                             LastChanged=r.LastChanged


                         }).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("Variable");
            return result;
        }

        public void Insert(VariableDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new VariableEntity
            {
                Id = data.Id,
                TenantId=data.TenantId,
                ItemId = data.ItemId,
                Token = data.Token,
                Value = data.Value,
                LastChanged = data.LastChanged!
            };
            MockDb.Variables.Add(newItem);
        }

        public void Update(VariableDto data)
        {
            var result = (from r in MockDb.Variables
                          where r.ItemId == data.ItemId && r.Id == data.Id 
                          && r.TenantId==data.TenantId
                          select r).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("Variable");
            if (!result.LastChanged.Matches(data.LastChanged!))
                throw new ConcurrencyException("Variable");
            data.LastChanged = MockDb.GetTimeStamp();
            result.Value = data.Value;
            result.LastChanged = data.LastChanged;
            
        }
    }
}
