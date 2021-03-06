﻿using BudgetAttempt.Finance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetAttempt.Finance.Helpers
{
    public class Filter
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Person Owner { get; set; }
        public FinanceType FinanceType { get; set; }
        public string Keywords { get; set; }
    }
}
