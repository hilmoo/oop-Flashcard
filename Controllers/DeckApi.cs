using Microsoft.AspNetCore.Mvc;
using flashcard.model.Entities;
using flashcard.Data;

namespace flashcard.Controllers
{
    [ApiController]
    [Route("api/v1/decks")]  // Changed to a more RESTful route
    public class DeckController : ControllerBase
    {
        private readonly FlashCardService _flashCardService;

        // Inject the FlashCardService through constructor
        public DeckController(FlashCardService flashCardService)
        {
            _flashCardService = flashCardService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Deck>>> GetDecks([FromQuery] string? email = null)
        {
            try
            {
                var decks = await _flashCardService.GetAllDecks(email!);
                return Ok(decks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Deck>> GetDeck(int id)
        {
            try
            {
                var flashcard = await _flashCardService.GetFlashcardByDeckId(id);
                return Ok(flashcard);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // [HttpPost]
        // public async Task<ActionResult<Deck>> CreateDeck([FromBody] Deck deck)
        // {
        //     // Implement create deck logic here
        //     return NotImplemented();
        // }
    }
}