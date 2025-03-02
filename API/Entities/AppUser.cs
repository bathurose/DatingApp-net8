using API.Extensions;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int> //using primary key type int instead of string (guid)
    {
        //public int Id { get; set; }
   
        //public required string UserName { get; set; }
        //public byte[] PasswordHash { get; set; } = [];
        //public byte[] PasswordSalt { get; set; } = [];
        public DateOnly DateOfBirth { get; set; }
        public required string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public required string Gender { get; set; }
        public string? Introduction { get; set; }
        public string? Interests { get; set; }
        public string? LookingFor { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public List<Photo> Photos { get; set; } = []; // 1- many

        public List<UserLike> LikedByUsers { get; set; } = []; // many - many
        public List<UserLike> LikedUsers { get; set; } = []; // many - many

        public List<Message> MessagesSent { get; set; } = []; // many - many
        public List<Message> MessagesReceived { get; set; } = []; // many - many

        public ICollection<AppUserRole> UserRoles { get; set; } = [];

        //public int GetAge()
        //{
        //    return DateOfBirth.CalculateAge();
        //} 
    }
}
