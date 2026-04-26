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

        // GET: api/favorites
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var favorites = await _favoriteService.GetAllAsync();
            return Ok(favorites);
        }

        // GET: api/favorites/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var favorite = await _favoriteService.GetByIdAsync(id);
            if (favorite == null)
                return NotFound($"Favorite song with id {id} not found");

            return Ok(favorite);
        }

        // POST: api/favorites
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FavoriteSong song)
        {
            if (string.IsNullOrWhiteSpace(song.Title))
                return BadRequest("Title is required");

            var created = await _favoriteService.AddAsync(song);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/favorites/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FavoriteSong song)
        {
            if (id != song.Id)
                return BadRequest("ID mismatch");

            var updated = await _favoriteService.UpdateAsync(id, song);
            if (updated == null)
                return NotFound($"Favorite song with id {id} not found");

            return Ok(updated);
        }

        // DELETE: api/favorites/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _favoriteService.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Favorite song with id {id} not found");

            return NoContent();
        }
    }
}