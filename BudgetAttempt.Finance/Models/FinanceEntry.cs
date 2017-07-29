using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetAttempt.Finance.Models
{
    public class FinanceEntry
    {
        public int Id { get; set; }
        public FinanceType FinanceType { get; set; }
        public DateTime Date { get; set; }
        public Person Person { get; set; }
        public FinanceCategory Category { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
    }
}
