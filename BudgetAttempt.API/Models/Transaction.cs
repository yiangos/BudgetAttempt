using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BudgetAttempt.API.Models
{
    [DataContract]
    public class Transaction
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string TransactionType { get; set; }
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public Person Person { get; set; }
        [DataMember]
        public TransactionCategory Category { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public string Comment { get; set; }
    }
}