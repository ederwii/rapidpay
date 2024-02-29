using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models.Base;

namespace Domain.Models
{
    public class Transaction : ModelBase
    {
        public Guid CardId { get; set; }
        public decimal Amount { get; set; }
    }
}