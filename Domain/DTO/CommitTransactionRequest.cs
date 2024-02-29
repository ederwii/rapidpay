using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class CommitTransactionRequest
    {
        public string CardCode { get; set; }
        public decimal Amount { get; set; }
    }
}