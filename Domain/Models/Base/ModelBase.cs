using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models.Base
{
    public class ModelBase
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}