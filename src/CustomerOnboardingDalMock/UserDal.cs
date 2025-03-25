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
                              PhoneNo=r.PhoneNo,
                              Email=r.Email,
                              Password=r.Password
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
                PhoneNo = data.PhoneNo,
                Email = data.Email,
                Password = data.Password,
                LastChanged = data.LastChanged
            };
            MockDb.Users.Add(newItem);
        }
    }
}
