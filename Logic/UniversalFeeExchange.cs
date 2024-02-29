using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces.Services;

namespace Logic
{
    public class UniversalFeesExchange : IUniversalFeesExchange
    {
        private decimal _currentFee = 1;
        private DateTime _lastUpdate = DateTime.MinValue;
        private readonly Random _random = new Random();
        private readonly object _lock = new object();

        public decimal GetCurrentFee()
        {
            lock (_lock)
            {
                if ((DateTime.Now - _lastUpdate).TotalHours >= 1)
                {
                    _currentFee *= (decimal)(_random.NextDouble() * 2);
                    _lastUpdate = DateTime.Now;
                }

                return _currentFee;
            }
        }

        private static UniversalFeesExchange _instance;
        public static UniversalFeesExchange Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UniversalFeesExchange();
                }
                return _instance;
            }
        }
    }

}