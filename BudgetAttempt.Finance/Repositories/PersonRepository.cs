using AutoMapper;
using BudgetAttempt.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetAttempt.Finance.Models;

namespace BudgetAttempt.Finance.Repositories
{
    public class PersonRepository : Interfaces.IPersonRepository
    {
        readonly IBudgetStorage _BudgetStorage;

        public PersonRepository(IBudgetStorage budgetStorage)
        {
            if (budgetStorage == null)
                throw new ArgumentNullException("budgetStorage");
            this._BudgetStorage = budgetStorage;
        }

        public Models.Person Create(Models.Person person)
        {
            var p = _BudgetStorage.CreatePerson(Mapper.Map<Data.Models.Person>(person));
            return Mapper.Map<Models.Person>(p);
        }

        public IEnumerable<Person> FetchAll()
        {
            var data = _BudgetStorage.FetchPersons();
            return Mapper.Map<IEnumerable<Person>>(data);
        }

        public Models.Person FetchOne(int Id)
        {
            var p = _BudgetStorage.FetchPerson(Id);
            if(p!=null)
            {
                return Mapper.Map<Models.Person>(p);
            }
            return null;
        }

        public Models.Person Update(Models.Person person)
        {
            var p = _BudgetStorage.UpdatePerson(Mapper.Map<Data.Models.Person>(person));
            return Mapper.Map<Models.Person>(p);
        }
    }
}
