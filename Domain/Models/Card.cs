using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models.Base;

namespace Domain.Models
{
    public class Card : ModelBase
    {
        public Card(string code)
        {
            Code = code;
        }
        public Guid CardId { get; set; } = Guid.NewGuid();
        public string Code { get; set; }
        public decimal Balance { get; set; } = 0;
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}