using BudgetAttempt.Finance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using AutoMapper;
using BudgetAttempt.API.Models;

namespace BudgetAttempt.API
{
    public class Budget : IBudget
    {
        readonly IFinanceRepository _FinanceRepository;
        readonly IPersonRepository _PersonRepository;

        public Budget(IFinanceRepository financeRepository, IPersonRepository personRepository)
        {
            this._FinanceRepository = financeRepository ?? throw new ArgumentNullException("financeRepository");
            this._PersonRepository = personRepository ?? throw new ArgumentNullException("personRepository");
        }

        #region Transactions

        public IEnumerable<Models.Transaction> GetLastMonthTransactionsJson()
        {
            return GetLastMonthTransactions();
        }

        public IEnumerable<Models.Transaction> GetLastMonthTransactionsXml()
        {
            return GetLastMonthTransactions();
        }

        public IEnumerable<Models.Transaction> GetLatestTransactionsJson(int count)
        {
            return GetLatestTransactions(count);
        }

        public IEnumerable<Models.Transaction> GetLatestTransactionsXml(int count)
        {
            return GetLatestTransactions(count);
        }

        private IEnumerable<Models.Transaction> GetLatestTransactions(int count)
        {
            return Mapper.Map<IEnumerable<Models.Transaction>>(_FinanceRepository.FetchAll(0, count));
        }

        private IEnumerable<Models.Transaction> GetLastMonthTransactions()
        {
            var filter = new BudgetAttempt.Finance.Models.Filter()
            {
                StartDate = DateTime.Now.AddMonths(-1),
                EndDate = DateTime.Now
            };
            return Mapper.Map<IEnumerable<Models.Transaction>>(_FinanceRepository.FetchFiltered(filter));
        }

        public Transaction AddTransactionJson(Transaction data)
        {
            return AddTransaction(data);
        }

        public Transaction AddTransactionXml(Transaction data)
        {
            return AddTransaction(data);
        }

        private Transaction AddTransaction(Transaction data)
        {
            var t = Mapper.Map<Finance.Models.FinanceEntry>(data);
            t = _FinanceRepository.CreateEntry(t);
            return Mapper.Map<Transaction>(t);
        }

        public IEnumerable<Models.Transaction> GetMonthTransactionsJson(string yearmonth)
        {
            return GetMonthTransactions(yearmonth);
        }

        public IEnumerable<Models.Transaction> GetMonthTransactionsXml(string yearmonth)
        {
            return GetMonthTransactions(yearmonth);
        }

        private IEnumerable<Models.Transaction> GetMonthTransactions(string yearmonth)
        {
            var d= DateTime.ParseExact(yearmonth + "-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
            var filter = new BudgetAttempt.Finance.Models.Filter()
            {

                StartDate = d,
                EndDate =d.AddMonths(1).AddDays(-1),
            };
            return Mapper.Map<IEnumerable<Models.Transaction>>(_FinanceRepository.FetchFiltered(filter));
        }

        #endregion

        #region persons
        public IEnumerable<Person> GetAllPersonsJson()
        {
            return GetAllPersons();
        }

        public IEnumerable<Person> GetAllPersonsXml()
        {
            return GetAllPersons();
        }

        public IEnumerable<Person> GetAllPersons()
        {
            return Mapper.Map<IEnumerable<Models.Person>>(_PersonRepository.FetchAll());
        }

        public Models.Person MergePersonJson(Models.Person data)
        {
            return MergePerson(data);
        }

        public Models.Person MergePersonXml(Models.Person data)
        {
            return MergePerson(data);
        }

        public Person MergePerson(Models.Person data)
        {
            var p = _PersonRepository.FetchOne(data.Id);
            if (p != null)
            {
                p.FirstName = data.FirstName;
                p.LastName = data.LastName;
                p = _PersonRepository.Update(p);
            }
            else
            {
                p = Mapper.Map<Finance.Models.Person>(data);
                p = _PersonRepository.Create(p);
            }
            return Mapper.Map<Models.Person>(p);
        }

        #endregion

        #region categories

        public TransactionCategory CreateCategoryJson(TransactionCategory data)
        {
            return CreateCategory(data);
        }

        public TransactionCategory CreateCategoryXml(TransactionCategory data)
        {
            return CreateCategory(data);
        }

        public TransactionCategory CreateCategory(TransactionCategory data)
        {
            var c = _FinanceRepository.FetchCategoryByNameAndType(Mapper.Map<Finance.Models.FinanceCategory>(data));
            if (c == null)
            {
                c = Mapper.Map<Finance.Models.FinanceCategory>(data);
                c = _FinanceRepository.CreateCategory(c);
            }
            return Mapper.Map<TransactionCategory>(c);
        }

        public IEnumerable<TransactionCategory> GetAllCategoriesJson()
        {
            return GetAllCategories();
        }

        public IEnumerable<TransactionCategory> GetAllCategoriesXml()
        {
            return GetAllCategories();
        }

        public IEnumerable<TransactionCategory> GetAllCategories()
        {
            return Mapper.Map<IEnumerable<Models.TransactionCategory>>(_FinanceRepository.FetchAllCategories());
        }

        public void GetOptions()
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept, Authorization");
        }

        #endregion

    }
}
