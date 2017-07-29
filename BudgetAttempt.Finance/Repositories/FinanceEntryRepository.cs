using BudgetAttempt.Finance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetAttempt.Finance.Models;
using BudgetAttempt.Data.Interfaces;
using AutoMapper;
using BudgetAttempt.Data.Models;

namespace BudgetAttempt.Finance.Repositories
{
    public class FinanceEntryRepository : IFinanceRepository
    {
        readonly IBudgetStorage _BudgetStorage;

        public FinanceEntryRepository(IBudgetStorage budgetStorage)
        {
            if (budgetStorage == null)
                throw new ArgumentNullException("budgetStorage");
            this._BudgetStorage = budgetStorage;
        }

        public IEnumerable<FinanceEntry> FetchAll()
        {
            return FetchFiltered(null);
        }

        public IEnumerable<FinanceEntry> FetchAll(int Page, int PerPage)
        {
            return FetchFiltered(null, Page, PerPage);
        }

        public IEnumerable<FinanceEntry> FetchFiltered(Filter filter)
        {
            return FetchFiltered(filter, 0, -1);
        }

        public IEnumerable<FinanceEntry> FetchFiltered(Filter filter, int Page, int PerPage)
        {
            var budgetFilter = new BudgetFilter();
            if (PerPage>0)
            {
                budgetFilter.PerPage = PerPage;
                budgetFilter.Page = Page;
            }

            if (filter!=null)
            {
                if (filter.StartDate > DateTime.MinValue)
                    budgetFilter.StartDate = filter.StartDate;
                if (filter.EndDate > DateTime.MinValue)
                    budgetFilter.EndDate = filter.EndDate;
                if (!string.IsNullOrEmpty(filter.Keywords))
                    budgetFilter.Keywords = filter.Keywords;
                if (filter.Owner!=null)
                    budgetFilter.OwnerId = filter.Owner.Id;
                if (filter.FinanceType != FinanceType.All)
                    budgetFilter.BudgetType = (int)filter.FinanceType;
                if (filter.Category != null)
                    budgetFilter.BudgetType = filter.Category.Id;
            }
            var data = _BudgetStorage.FetchLines(budgetFilter);
            return Mapper.Map<IEnumerable<FinanceEntry>>(data);
        }

        public FinanceEntry FetchEntry(int Id)
        {
            var l = _BudgetStorage.FetchLine(Id);
            if (l != null)
            {
                return Mapper.Map<Models.FinanceEntry>(l);
            }
            return null;
        }

        public FinanceCategory FetchCategory(int Id)
        {
            var l = _BudgetStorage.FetchCategory(Id);
            if (l != null)
            {
                return Mapper.Map<Models.FinanceCategory>(l);
            }
            return null;
        }

        public IEnumerable<FinanceEntry> FetchCategoriesOfType(FinanceType type)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FinanceCategory> FetchAllCategories()
        {
            var data = _BudgetStorage.FetchCategories(null);
            return Mapper.Map<IEnumerable<FinanceCategory>>(data);
        }

        public FinanceCategory FetchCategoryByNameAndType(FinanceCategory Filter)
        {
            var c = _BudgetStorage.FetchCategoryByNameAndType(Mapper.Map<Data.Models.Category>(Filter));
            if (c != null)
            {
                return Mapper.Map<Models.FinanceCategory>(c);
            }
            return null;
        }

        public FinanceCategory CreateCategory(FinanceCategory data)
        {
            var c = _BudgetStorage.CreateCategory(Mapper.Map<Data.Models.Category>(data));
            if (c != null)
            {
                return Mapper.Map<Models.FinanceCategory>(c);
            }
            return null;
        }

        public FinanceEntry CreateEntry(FinanceEntry data)
        {
            var l = _BudgetStorage.CreateLine(Mapper.Map<Data.Models.BudgetLine>(data));
            if (l != null)
            {
                return Mapper.Map<Models.FinanceEntry>(l);
            }
            return null;
        }
    }
}
