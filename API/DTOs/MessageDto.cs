namespace API.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public required string SenderUserName { get; set; }
        public required string SenderUserPhotoUrl { get; set; }
        public int RecipientId { get; set; }
        public required string RecipientUserName { get; set; }
        public required string RecipientUserPhotoUrl { get; set; }

        public required string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.UtcNow;
    }
}
