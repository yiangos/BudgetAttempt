using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetAttempt.Data.Models
{
    public class BudgetLine
    {
        public int Id { get; set; }
        public int BudgetType { get; set; }
        public DateTime Date { get; set; }
        public int person { get; set; }
        public int category { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
    }
}
