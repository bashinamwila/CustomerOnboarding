using CustomerOnboarding.Dal.Dtos;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IAddDeductionsStepDal
    {
        public void Insert(AddDeductionsStepDto dto);
        public void Update(AddDeductionsStepDto dto);

        public AddDeductionsStepDto Fetch(string tenantId, int id);
    }
}