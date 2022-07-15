namespace API.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public AppUser Sender { get; set; }
        public int RecipientId { get; set; }
        public string RecipientName { get; set; }
        public AppUser Recipient { get; set; }
        public string Content { get; set; }
        public DateTime? ReadTime { get; set; }
        public DateTime SendTime { get; set; } = DateTime.UtcNow;
        public bool SenderDel { get; set; }
        public bool RecipientDel { get; set; }
    }
}
