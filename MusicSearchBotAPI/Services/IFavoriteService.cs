using MusicSearchBotAPI.Models.Favorite;

namespace MusicSearchBotAPI.Services
{
    public interface IFavoriteService
    {
        Task<List<FavoriteSong>> GetAllAsync(long userId);
        Task<FavoriteSong?> GetByIdAsync(long userId, int id);
        Task<FavoriteSong> AddAsync(long userId, FavoriteSong song);
        Task<FavoriteSong?> UpdateAsync(long userId, int id, FavoriteSong song);
        Task<bool> DeleteAsync(long userId, int id);
    }
}