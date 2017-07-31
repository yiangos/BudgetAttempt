using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetAttempt.API.Resolvers
{
    public interface IFinanceCategoryResolver { }
    public class FinanceCategoryResolver : IFinanceCategoryResolver, IValueResolver<Data.Models.BudgetLine,Finance.Models.FinanceEntry, Finance.Models.FinanceCategory>
    {
        readonly Finance.Interfaces.IFinanceRepository _FinanceRepository;

        public FinanceCategoryResolver(Finance.Interfaces.IFinanceRepository financeRepository)
        {
            if (financeRepository == null)
                throw new ArgumentNullException("financeRepository");
            this._FinanceRepository = financeRepository;
        }

        public Finance.Models.FinanceCategory Resolve(Data.Models.BudgetLine source, Finance.Models.FinanceEntry destination, Finance.Models.FinanceCategory category, ResolutionContext context)
        {
            return _FinanceRepository.FetchCategory(source.category);
        }
    }
}
