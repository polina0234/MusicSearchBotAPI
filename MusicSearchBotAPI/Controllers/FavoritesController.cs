using Microsoft.AspNetCore.Mvc;
using MusicSearchBotAPI.Services;
using MusicSearchBotAPI.Models.Favorite;

namespace MusicSearchBotAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoritesController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromHeader] long userId)
        {
            var favorites = await _favoriteService.GetAllAsync(userId);
            return Ok(favorites);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, [FromHeader] long userId)
        {
            var favorite = await _favoriteService.GetByIdAsync(userId, id);

            if (favorite == null)
                return NotFound($"Favorite song with id {id} not found");

            return Ok(favorite);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FavoriteSong song, [FromHeader] long userId)
        {
            if (string.IsNullOrWhiteSpace(song.Title))
                return BadRequest("Title is required");

            var created = await _favoriteService.AddAsync(userId, song);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FavoriteSong song, [FromHeader] long userId)
        {
            if (id != song.Id)
                return BadRequest("ID mismatch");

            var updated = await _favoriteService.UpdateAsync(userId, id, song);

            if (updated == null)
                return NotFound($"Favorite song with id {id} not found");

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromHeader] long userId)
        {
            var deleted = await _favoriteService.DeleteAsync(userId, id);

            if (!deleted)
                return NotFound($"Favorite song with id {id} not found");

            return NoContent();
        }
    }
}