using API.Extensions;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public required string UserName { get; set; }

        public required byte[] PasswordHash { get; set; }

    }
}
