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
    public class UserDal : IUserDal
    {
        public UserDto Fetch(string id)
        {
            var result = (from r in MockDb.Users
                          where r.TenantId == id
                          select new UserDto
                          {
                              FirstName=r.FirstName,
                              LastName=r.LastName,
                              PhoneNumber=r.PhoneNo,
                              Email=r.Email,
                              Password=r.Password,
                              IsConfirmed= r.IsConfirmed,
                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("User");
            return result;
        }

        public void Insert(UserDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new UserEntity
            {
                TenantId = data.TenantId,
                FirstName = data.FirstName,
                LastName = data.LastName,
                PhoneNo = data.PhoneNumber,
                Email = data.Email,
                Password = data.Password,
                IsConfirmed = data.IsConfirmed,
                LastChanged = data.LastChanged
            };
            MockDb.Users.Add(newItem);
        }

        public void Update(UserDto data)
        {
            var result=(from r in MockDb.Users
                        where r.TenantId == data.TenantId
                        && r.Email == data.Email
                        select r).FirstOrDefault();
            if(result is null)
                throw new DataNotFoundException("User");
            if(!result.LastChanged.Matches(data.LastChanged))
                throw new ConcurrencyException("User");
            data.LastChanged = MockDb.GetTimeStamp();
            result.IsConfirmed = data.IsConfirmed;
        }
    }
}
