using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetAttempt.Finance.Interfaces
{
    public interface IPersonRepository
    {
        IEnumerable<Models.Person> FetchAll();
        Models.Person FetchOne(int id);
        Models.Person Create(Models.Person person);
        Models.Person Update(Models.Person person);
    }
}
