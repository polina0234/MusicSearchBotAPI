using Microsoft.AspNetCore.Mvc;
using MusicSearchBotAPI.Services;
using BotMusicSearch.Models;

namespace MusicSearchBotAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MusicController : ControllerBase
    {
        private readonly IMusicService _musicService;
        public MusicController(IMusicService musicService)
        {
            _musicService = musicService;
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query parameter is required.");
            }
            var results = await _musicService.SearchMusicAsync(query);
            return Ok(results);
        }
    

    [HttpGet("details/{videoId}")]
        public async Task<IActionResult> GetVideoDetails(string videoId)
        {
            if (string.IsNullOrWhiteSpace(videoId))
            {
                return BadRequest("Video ID is required.");
            }
            var details = await _musicService.GetVideoDetailsAsync(videoId);
            if (details == null || details.items == null || details.items.Length == 0)
            {
                return NotFound("Video not found.");
            }
            return Ok(details);
        }

        [HttpGet("related")]
        public async Task<IActionResult> GetRelated([FromQuery] string artist, [FromQuery] string title)
        {
            if (string.IsNullOrWhiteSpace(artist) || string.IsNullOrWhiteSpace(title))
            {
                return BadRequest("Artist name and song title are required.");
            }
            var related = await _musicService.GetRelatedSearchAsync(artist, title);
            if (related == null)
            {
                return NotFound("No related videos found.");
            }
            return Ok(related);
        }
    }
}
