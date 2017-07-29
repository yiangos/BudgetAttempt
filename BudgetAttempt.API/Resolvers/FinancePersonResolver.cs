using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetAttempt.API.Resolvers
{
    public interface IFinancePersonResolver { }
    public class FinancePersonResolver : IFinancePersonResolver, IValueResolver<Data.Models.BudgetLine, Finance.Models.FinanceEntry, Finance.Models.Person>
    {
        readonly Finance.Interfaces.IPersonRepository _PersonRepository;

        public FinancePersonResolver(Finance.Interfaces.IPersonRepository personRepository)
        {
            if (personRepository == null)
                throw new ArgumentNullException("personRepository");
            this._PersonRepository = personRepository;
        }

        public Finance.Models.Person Resolve(Data.Models.BudgetLine source, Finance.Models.FinanceEntry destination, Finance.Models.Person member, ResolutionContext context)
        {
            return Mapper.Map<Finance.Models.Person>(_PersonRepository.FetchOne(source.person));
        }
    }
}
