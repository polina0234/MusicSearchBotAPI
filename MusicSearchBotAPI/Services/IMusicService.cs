using System.Net.Http;  
using Newtonsoft.Json;  
using BotMusicSearch.Models; 

namespace MusicSearchBotAPI.Services
{
    public interface IMusicService
    {
        Task<List<Class1>> SearchMusicAsync(string query);
        Task<VideoDetailsResponse> GetVideoDetailsAsync(string videoId);
        Task<RelatedSearchResponse> GetRelatedSearchAsync(string artistName, string songTitle);
    }
}