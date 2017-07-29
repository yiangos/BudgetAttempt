using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetAttempt.Data.Interfaces
{
    public interface IBudgetStorage
    {
        IEnumerable<Models.BudgetLine> FetchLines(Models.BudgetFilter Filter);
        Models.BudgetLine FetchLine(int Id);
        Models.Person FetchPerson(int Id);
        Models.Category FetchCategory(int Id);
        Models.Category FetchCategoryByNameAndType(Models.Category category);
        IEnumerable<Models.Category> FetchCategories(int? BudgetType);
        Models.Person CreatePerson(Models.Person person);
        Models.Person UpdatePerson(Models.Person person);
        IEnumerable<Models.Person> FetchPersons();
        
        Models.Category CreateCategory(Models.Category category);
        Models.BudgetLine CreateLine(Models.BudgetLine data);
    }
}
