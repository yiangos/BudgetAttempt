using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetAttempt.Data.Models
{
    public class Category
    {
        public int Id { get; set; }
        public int BudgetType { get; set; }
        public string Description { get; set; }
    }
}
