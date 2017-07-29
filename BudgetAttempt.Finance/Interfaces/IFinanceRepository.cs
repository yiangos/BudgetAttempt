using BudgetAttempt.Finance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetAttempt.Finance.Interfaces
{
    public interface IFinanceRepository
    {
        IEnumerable<FinanceEntry> FetchAll();
        IEnumerable<FinanceEntry> FetchAll(int Page, int PerPage);
        FinanceEntry FetchEntry(int Id);
        IEnumerable<FinanceEntry> FetchFiltered(Filter filter);
        IEnumerable<FinanceEntry> FetchFiltered(Filter filter, int Page, int PerPage);
        FinanceEntry CreateEntry(FinanceEntry data);
        FinanceCategory FetchCategory(int Id);
        FinanceCategory FetchCategoryByNameAndType(FinanceCategory Filter);
        FinanceCategory CreateCategory(FinanceCategory data);
        IEnumerable<FinanceEntry> FetchCategoriesOfType(FinanceType type);
        IEnumerable<FinanceCategory> FetchAllCategories();

    }
}
