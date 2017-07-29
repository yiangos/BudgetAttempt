using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BudgetAttempt.API.Models
{
    [DataContract]
    public class TransactionCategory
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string TransactionType { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}