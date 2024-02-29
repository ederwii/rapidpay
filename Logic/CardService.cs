using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Domain.Models;
using Domain.DTO;
using Domain.Interfaces.Services;
using Domain.Interfaces.Repositories;

namespace Logic
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
        private readonly IRepository<Transaction> _transactionRepository;

        public CardService(ICardRepository cardRepository, IRepository<Transaction> transactionRepository)
        {
            _cardRepository = cardRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<Card> CreateAsync(CreateCardRequest request)
        {
            var cardCode = await GenerateUniqueCardCodeAsync();

            var card = new Card(cardCode);

            await _cardRepository.AddAsync(card);

            var initialTransaction = new CommitTransactionRequest
            {
                CardCode = card.Code,
                Amount = request.InitialBalance
            };

            await CommitTransactionAsync(initialTransaction);

            return card;
        }
        public async Task<Card> PayAsync(CommitTransactionRequest request)
        {
            if (await HasEnoughBalanceAsync(request))
            {
                if (request.Amount > 0)
                    request.Amount *= -1;
                return await CommitTransactionAsync(request);
            }
            else
            {
                throw new ArgumentException("Not enough balance.");
            }
        }
        public async Task<Card> CommitTransactionAsync(CommitTransactionRequest request)
        {
            var card = await _cardRepository.GetByCodeAsync(request.CardCode);
            if (card == null)
            {
                throw new ArgumentException("Card not found");
            }

            var transaction = new Transaction
            {
                CardId = card.CardId,
                Amount = request.Amount
            };

            await _transactionRepository.AddAsync(transaction);

            return await RecalculateBalanceAsync(card.CardId);

        }

        public async Task<Card> RecalculateBalanceAsync(Guid cardId)
        {
            var card = await _cardRepository.GetByIdAsync(cardId);
            if (card == null)
            {
                throw new ArgumentException("Card not found");
            }

            var newBalance = card.Transactions.Sum(t => t.Amount);

            card.Balance += newBalance;

            await _cardRepository.UpdateAsync(card);

            return card;
        }

        private async Task<bool> HasEnoughBalanceAsync(CommitTransactionRequest request)
        {
            var card = await _cardRepository.GetByCodeAsync(request.CardCode);
            if (card == null)
            {
                throw new ArgumentException("Card not found");
            }

            return card.Balance >= request.Amount;
        }


        private async Task<string> GenerateUniqueCardCodeAsync()
        {
            var uniqueCode = "";
            var random = new Random();
            do
            {
                uniqueCode = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 15)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            while (await _cardRepository.AnyAsync(uniqueCode));

            return uniqueCode;
        }

    }
}