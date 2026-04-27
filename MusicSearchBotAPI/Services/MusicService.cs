using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using BotMusicSearch.Models;
using System.Dynamic;
using Microsoft.Extensions.Configuration;

namespace MusicSearchBotAPI.Services
{
    public class MusicService : IMusicService
    {
        private readonly HttpClient _httpClient;
        private readonly string _youtubeApiKey;

        public MusicService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _youtubeApiKey = configuration["YoutubeApiKey"];
        }

        public async Task<RelatedSearchResponse> GetRelatedSearchAsync(string artistName, string songTitle)
        {
            string searchQuery = $"{artistName} {songTitle} official music video";
            string url = $"https://www.googleapis.com/youtube/v3/search?part=snippet&q={Uri.EscapeDataString(searchQuery)}&type=video&maxResults=8&key={_youtubeApiKey}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RelatedSearchResponse>(json);
        }

        public async Task<VideoDetailsResponse> GetVideoDetailsAsync(string videoId)
        {
            string url = $"https://www.googleapis.com/youtube/v3/videos?part=snippet,contentDetails,statistics&id={videoId}&key={_youtubeApiKey}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<VideoDetailsResponse>(json);
        }

        public async Task<List<Class1>> SearchMusicAsync(string query)
        {
            string url = $"https://www.googleapis.com/youtube/v3/search?part=snippet&maxResults=15&q={Uri.EscapeDataString(query)}&type=video&key={_youtubeApiKey}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return new List<Class1>();
            string json = await response.Content.ReadAsStringAsync();
            dynamic searchResult = JsonConvert.DeserializeObject(json);
            var songs = new List<Class1>();
            foreach (var item in searchResult.items)
            {
                var song = new Class1
                {
                    videoId = item.id.videoId,
                    title = item.snippet.title,
                    artists = new[] { new Artist { name = item.snippet.channelTitle, id = item.snippet.channelId } },
                    views = "N/A",
                    resultType = "song"
                };
                songs.Add(song);
            }
            return songs;
        }
    }
}