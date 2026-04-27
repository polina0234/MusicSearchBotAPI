using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using BotMusicSearch.Models;
using System.Dynamic;

namespace MusicSearchBotAPI.Services
{
    public class MusicService : IMusicService
    {
        private readonly HttpClient _httpClient;
        private readonly string _youtubeApiKey = "AIzaSyD4YZMgz6w_IUsCsRmWEw4KNFnapqy1j5w";

        public MusicService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RelatedSearchResponse> GetRelatedSearchAsync(string artistName, string songTitle)
        {
            string searchQuery = $"{artistName} {songTitle} official music video";
            string url = $"https://www.googleapis.com/youtube/v3/search?part=snippet&q={Uri.EscapeDataString(searchQuery)}&type=video&maxResults=8&key={_youtubeApiKey}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RelatedSearchResponse>(json);
        }

        public async Task<VideoDetailsResponse> GetVideoDetailsAsync(string videoId)
        {
            string url = $"https://www.googleapis.com/youtube/v3/videos?part=snippet,contentDetails,statistics&id={videoId}&key={_youtubeApiKey}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<VideoDetailsResponse>(json);
        }

        public async Task<List<Class1>> SearchMusicAsync(string query)
        {
            string url = $"https://www.googleapis.com/youtube/v3/search?part=snippet&maxResults=30&q={Uri.EscapeDataString(query)}&type=video&key={_youtubeApiKey}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new List<Class1>();

            string json = await response.Content.ReadAsStringAsync();
            dynamic searchResult = JsonConvert.DeserializeObject(json);

            var songs = new List<Class1>();
            foreach (var item in searchResult.items)
            {
                string title = ((string)item.snippet.title).ToLower();
                string channelTitle = ((string)item.snippet.channelTitle).ToLower();

                // Фільтрація: залишаємо тільки музику
                bool isMusic = (title.Contains("official") ||
                                title.Contains("music video") ||
                                title.Contains("audio") ||
                                title.Contains("lyrics") ||
                                title.Contains("official video") ||
                                title.Contains("track") ||
                                title.Contains("song") ||
                                channelTitle.Contains("topic") ||
                                channelTitle.Contains("vevo") ||
                                channelTitle.Contains("music")) &&
                               !title.Contains("live") &&
                               !title.Contains("reaction") &&
                               !title.Contains("podcast") &&
                               !title.Contains("interview") &&
                               !title.Contains("trailer") &&
                               !title.Contains("review") &&
                               !title.Contains("урок") &&
                               !title.Contains("лекція") &&
                               !title.Contains("обзор") &&
                               !title.Contains("наука") &&
                               !title.Contains("эксперимент") &&
                               !title.Contains("аккумулятор") &&
                               !title.Contains("батарейка") &&
                               !title.Contains("фокус") &&
                               !title.Contains("проводка");

                if (isMusic)
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
            }

            return songs;
        }
    }
}