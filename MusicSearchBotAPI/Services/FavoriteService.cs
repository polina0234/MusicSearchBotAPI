using System.Text.Json;
using MusicSearchBotAPI.Models.Favorite;

namespace MusicSearchBotAPI.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Storage", "favorites.json");
        private int _nextId = 1;

        public FavoriteService()
        {
            InitializeFile();
        }

        private void InitializeFile()
        {
            var directory = Path.GetDirectoryName(_filePath);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
            else
            {
                var songs = ReadFromFileAsync().GetAwaiter().GetResult();
                if (songs.Any())
                    _nextId = songs.Max(s => s.Id) + 1;
            }
        }

        private async Task<List<FavoriteSong>> ReadFromFileAsync()
        {
            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<FavoriteSong>>(json) ?? new List<FavoriteSong>();
        }

        private async Task WriteToFileAsync(List<FavoriteSong> songs)
        {
            var json = JsonSerializer.Serialize(songs, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await File.WriteAllTextAsync(_filePath, json);
        }

        public async Task<List<FavoriteSong>> GetAllAsync(long userId)
        {
            var songs = await ReadFromFileAsync();
            return songs.Where(s => s.UserId == userId).ToList();
        }

        public async Task<FavoriteSong?> GetByIdAsync(long userId, int id)
        {
            var songs = await ReadFromFileAsync();
            return songs.FirstOrDefault(s => s.Id == id && s.UserId == userId);
        }

        public async Task<FavoriteSong> AddAsync(long userId, FavoriteSong song)
        {
            var songs = await ReadFromFileAsync();

            song.Id = _nextId++;
            song.UserId = userId;
            song.DateAdded = DateTime.Now;

            songs.Add(song);
            await WriteToFileAsync(songs);

            return song;
        }

        public async Task<FavoriteSong?> UpdateAsync(long userId, int id, FavoriteSong updatedSong)
        {
            var songs = await ReadFromFileAsync();

            var index = songs.FindIndex(s => s.Id == id && s.UserId == userId);
            if (index == -1)
                return null;

            updatedSong.Id = id;
            updatedSong.UserId = userId;
            updatedSong.DateAdded = songs[index].DateAdded;

            songs[index] = updatedSong;

            await WriteToFileAsync(songs);
            return updatedSong;
        }

        public async Task<bool> DeleteAsync(long userId, int id)
        {
            var songs = await ReadFromFileAsync();

            var song = songs.FirstOrDefault(s => s.Id == id && s.UserId == userId);
            if (song == null)
                return false;

            songs.Remove(song);
            await WriteToFileAsync(songs);

            return true;
        }
    }
}