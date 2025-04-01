using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IUserDal
    {
        public void Insert(UserDto data);
        public void Update(UserDto data);
        public UserDto Fetch(string id);
    }
}
