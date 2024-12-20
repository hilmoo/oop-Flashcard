using Microsoft.AspNetCore.Mvc;
using flashcard.model.Entities;
using flashcard.Data;

namespace flashcard.Controllers;

[ApiController]
[Route("api/v1/decks")] // Changed to a more RESTful route
public class DeckController(FlashCardService flashCardService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Deck>>> GetDecks([FromQuery] string? email = null)
    {
        try
        {
            var decks = await flashCardService.GetAllDecks(email!);
            return Ok(decks);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Deck>> GetDeck(int id)
    {
        try
        {
            var flashcard = await flashCardService.GetFlashcardByDeckId(id);
            return Ok(flashcard);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}