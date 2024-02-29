using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Base;

namespace Data
{
    public class Card : EntityBase
    {
        public string Code { get; set; }
        public decimal Balance { get; set; }
    }
}