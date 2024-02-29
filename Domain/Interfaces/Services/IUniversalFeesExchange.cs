using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
    public interface IUniversalFeesExchange
    {
        decimal GetCurrentFee();
    }
}