namespace MusicSearchBotAPI.Models.Favorite
{
    public class FavoriteSong
    {
        public int Id { get; set; }
        public long UserId { get; set; }

        public string VideoId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }

        public DateTime DateAdded { get; set; }
    }
}