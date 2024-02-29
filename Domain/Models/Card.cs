using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models.Base;

namespace Domain.Models
{
    public class Card : ModelBase
    {
        public string Code { get; set; }
        public decimal Balance { get; set; }
    }
}