using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Domain.DTO;
using Domain.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace WebApi.IntegrationTests
{
    [TestFixture]
    public class CardControllerTests
    {
        private HttpClient _client;
        private CustomWebApplicationFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
            _factory?.Dispose();
        }

        [Test]
        public async Task CreateCard_WithZeroBalance_AndTryPayment_ShouldReturnBadRequest()
        {
            var createResponse = await _client.PostAsJsonAsync("/api/Card/Create", new CreateCardRequest { InitialBalance = 100 });
            var card = await createResponse.Content.ReadAsAsync<Card>();
            Assert.That(createResponse.IsSuccessStatusCode, Is.True);

            Assert.That(card.Transactions.Count == 1, "There should be a single transaction");

            var payResponse = await _client.PostAsJsonAsync("/api/Card/Pay", new CommitTransactionRequest { CardCode = card.Code, Amount = 101 });
            Assert.That(payResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest), "Not nough balance operation should return bad request");

            var payResponse2 = await _client.PostAsJsonAsync("/api/Card/Pay", new CommitTransactionRequest { CardCode = card.Code, Amount = 50 });
            Assert.That(payResponse2.IsSuccessStatusCode);
            card = await payResponse2.Content.ReadAsAsync<Card>();
            Assert.That(card.Balance < 50, "New balance should be less than 50 due to the payment fee");

            var payResponse3 = await _client.PostAsJsonAsync("/api/Card/Pay", new CommitTransactionRequest { CardCode = card.Code, Amount = card.Balance + 1 });
            Assert.That(payResponse3.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest), "Not nough balance operation should return bad request");
        }
    }
}