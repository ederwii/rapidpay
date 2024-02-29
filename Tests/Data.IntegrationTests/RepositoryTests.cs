using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Repositories;
using Domain.Models;

namespace Data.IntegrationTests
{
    [TestFixture]
    public class RepositoryTests
    {
        [Test]
        public async Task AddAsync_AddsEntity()
        {
            using var context = DbContextFactory.CreateDbContext();
            var repository = new Repository<Card>(context);

            var card = new Card { Code = "Test-Code", Balance = 100.0m };

            var addedCard = await context.Set<Card>().FindAsync(card.CardId);
            Assert.IsNull(addedCard);

            await repository.AddAsync(card);

            addedCard = await context.Set<Card>().FindAsync(card.CardId);
            Assert.IsNotNull(addedCard);
        }

        [Test]
        public async Task GetAllAsync_RetrievesAllEntities()
        {
            using var context = DbContextFactory.CreateDbContext();
            var repository = new Repository<Card>(context);

            await repository.AddAsync(new Card { Code = "Test-Code1", Balance = 100.0m });
            await repository.AddAsync(new Card { Code = "Test-Code2", Balance = 200.0m });

            var cards = await repository.GetAllAsync();

            Assert.AreEqual(2, cards.Count());
        }

        [Test]
        public async Task GetByIdAsync_RetrievesCorrectEntity()
        {
            using var context = DbContextFactory.CreateDbContext();
            var repository = new Repository<Card>(context);

            var card = new Card { Code = "Test-Code", Balance = 100.0m };
            await repository.AddAsync(card);

            var retrievedCard = await repository.GetByIdAsync(card.CardId);

            Assert.IsNotNull(retrievedCard);
            Assert.AreEqual(card.CardId, retrievedCard.CardId);
        }

        [Test]
        public async Task UpdateAsync_UpdatesEntityCorrectly()
        {
            using var context = DbContextFactory.CreateDbContext();
            var repository = new Repository<Card>(context);

            var card = new Card { Code = "Test-Code", Balance = 100.0m };
            await repository.AddAsync(card);

            card.Balance = 200.0m;
            await repository.UpdateAsync(card);

            var updatedCard = await repository.GetByIdAsync(card.CardId);

            Assert.IsNotNull(updatedCard);
            Assert.AreEqual(200.0m, updatedCard.Balance);
        }

        [Test]
        public async Task DeleteAsync_RemovesEntity()
        {
            using var context = DbContextFactory.CreateDbContext();
            var repository = new Repository<Card>(context);

            var card = new Card { Code = "Test-Code", Balance = 100.0m };
            await repository.AddAsync(card);

            var addedCard = await repository.GetByIdAsync(card.CardId);
            Assert.IsNotNull(addedCard);

            await repository.DeleteAsync(card);

            var deletedCard = await repository.GetByIdAsync(card.CardId);
            Assert.IsNull(deletedCard);
        }

        [TearDown]
        public void Cleanup()
        {
            using var context = DbContextFactory.CreateDbContext();
            context.Database.EnsureDeleted(); 
        }

    }
}