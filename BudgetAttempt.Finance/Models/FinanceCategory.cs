using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetAttempt.Finance.Models
{
    public class FinanceCategory
    {
        public int Id { get; set; }
        public FinanceType FinanceType { get; set; }
        public string Description { get; set; }
    }
}
