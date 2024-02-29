using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Domain.Models;
using Domain.DTO;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Logic;
using System.Collections.Generic;

namespace Logic.UnitTests
{
    [TestFixture]
    public class CardServiceTests
    {
        private Mock<ICardRepository> _mockCardRepository;
        private Mock<IRepository<Transaction>> _mockTransactionRepository;
        private Mock<IUniversalFeesExchange> _mockIUniversalFeesExchange;
        private ICardService _cardService;

        [SetUp]
        public void Setup()
        {
            _mockCardRepository = new Mock<ICardRepository>();
            _mockIUniversalFeesExchange = new Mock<IUniversalFeesExchange>();
            _mockTransactionRepository = new Mock<IRepository<Transaction>>();
            _cardService = new CardService(_mockCardRepository.Object, _mockTransactionRepository.Object, _mockIUniversalFeesExchange.Object);
        }

        [Test]
        public async Task CreateAsync_ValidRequest_CreatesCardWithInitialBalance()
        {
            var card = new Card("1234");
            var request = new CreateCardRequest { InitialBalance = 100 };
            _mockCardRepository.Setup(repo => repo.GetByCodeAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(card));
            _mockCardRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(card));
            _mockCardRepository.Setup(repo => repo.AnyAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(false));
            _mockCardRepository.Setup(repo => repo.AddAsync(It.IsAny<Card>()))
                .Returns(Task.FromResult(card));
            _mockTransactionRepository.Setup(repo => repo.AddAsync(It.IsAny<Transaction>()))
                .Returns(Task.FromResult(It.IsAny<Transaction>()));

            var newCard = await _cardService.CreateAsync(request);

            Assert.IsNotNull(card);
            _mockCardRepository.Verify(repo => repo.AddAsync(It.IsAny<Card>()), Times.Once);
            _mockTransactionRepository.Verify(repo => repo.AddAsync(It.IsAny<Transaction>()), Times.Once);
        }

        [Test]
        public async Task CommitTransaction_AddsTransactionSuccessfully()
        {
            var card = new Card("UNIQUECODE123");
            var request = new CommitTransactionRequest { CardCode = card.Code, Amount = 50 };
            _mockCardRepository.Setup(repo => repo.GetByCodeAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(card));
            _mockCardRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(card));
            _mockTransactionRepository.Setup(repo => repo.AddAsync(It.IsAny<Transaction>()))
                .Returns(Task.FromResult(It.IsAny<Transaction>()));
            _mockCardRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Card>())).Returns(Task.FromResult(card));

            var result = await _cardService.CommitTransactionAsync(request);

            _mockTransactionRepository.Verify(repo => repo.AddAsync(It.IsAny<Transaction>()), Times.Once);
        }

        [Test]
        public async Task RecalculateBalance_UpdatesBalanceCorrectly()
        {
            var transactions = new List<Transaction>
                {
                    new Transaction { Amount = 100 },
                    new Transaction { Amount = -50 }
                };
            var card = new Card("Test Code");
            _mockCardRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(card);
            _mockCardRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Card>())).Returns(Task.CompletedTask);

            var result = await _cardService.RecalculateBalanceAsync(Guid.NewGuid());

            _mockCardRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Card>()), Times.Once);
        }
    }
}