using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Interfaces.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        // POST: api/Card/Create
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateCardRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var card = await _cardService.CreateAsync(request);
                return Ok(card);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Card/Pay
        [HttpPost("Pay")]
        public async Task<IActionResult> Pay([FromBody] CommitTransactionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (request.Amount <= 0)
                {
                    return BadRequest("Amount must be greater than zero.");
                }

                var card = await _cardService.PayAsync(request);
                if (card == null)
                {
                    return NotFound("Card not found.");
                }

                return Ok(card);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
