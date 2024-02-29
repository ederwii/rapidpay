using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Data;
using Domain.Models;
using Domain.Interfaces.Repositories;

namespace Data.Repositories
{
    public class CardRepository : Repository<Card>, ICardRepository
    {
        private readonly AppDbContext _context;

        public CardRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Card> GetByCodeAsync(string code)
        {
            return await _context.Cards.FirstOrDefaultAsync(c => c.Code == code);
        }

        public async Task<bool> AnyAsync(string code)
        {
            return await _context.Cards.AnyAsync(c => c.Code == code);
        }
    }
}