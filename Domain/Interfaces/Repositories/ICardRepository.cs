using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces.Repositories
{
    public interface ICardRepository : IRepository<Card>
    {
        Task<Card> GetByCodeAsync(string code);
        Task<bool> AnyAsync(string code);
    }
}