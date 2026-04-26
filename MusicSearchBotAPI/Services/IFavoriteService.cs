using MusicSearchBotAPI.Models.Favorite;

namespace MusicSearchBotAPI.Services
{
    public interface IFavoriteService
    {
        Task<List<FavoriteSong>> GetAllAsync();
        Task<FavoriteSong?> GetByIdAsync(int id);
        Task<FavoriteSong> AddAsync(FavoriteSong song);
        Task<FavoriteSong?> UpdateAsync(int id, FavoriteSong song);
        Task<bool> DeleteAsync(int id);
    }
}