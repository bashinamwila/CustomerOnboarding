using CustomerOnboarding.Dal.Dtos;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IAddEmployerExpensesStepDal
    {
        public void Insert(AddEmployerExpensesStepDto dto);
        public void Update(AddEmployerExpensesStepDto dto);

        public AddEmployerExpensesStepDto Fetch(string tenantId, int id);
    }
}