using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Domain.DTO;

namespace Domain.Interfaces.Services
{
    public interface ICardService
    {
        Task<Card> CreateAsync(CreateCardRequest request);
        Task<Card> CommitTransaction(CommitTransactionRequest request);
        Task<Card> RecalculateBalance(Guid cardId);
    }
}