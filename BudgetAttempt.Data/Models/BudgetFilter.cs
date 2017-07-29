using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetAttempt.Data.Models
{
    public class BudgetFilter
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? OwnerId { get; set; }
        public int? BudgetType { get; set; }
        public int? BudgetCategory { get; set; }
        public int? Page { get; set; }
        public int? PerPage { get; set; }
        public string Keywords { get; set; }
    }
}
