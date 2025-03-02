namespace API.Helper
{
    public class MessageParams : PaginationParams
    {
        public required string UserName {  get; set; }
        public string Container { get; set; } = "Unread";
    }
}
