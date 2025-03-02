using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Connection
    {
        [Key]
        public required string ConntectionId { get; set; }
        public required string Username {  get; set; }
    }
}
