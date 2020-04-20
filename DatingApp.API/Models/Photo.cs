namespace DatingApp.API.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}