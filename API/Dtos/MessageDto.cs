using System.Text.Json.Serialization;

namespace API.Dtos
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderPhotoUrl { get; set; }
        public int RecipientId { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhotoUrl { get; set; }

        public string Content { get; set; }
        public DateTime? ReadTime { get; set; }
        public DateTime SendTime { get; set; }

        [JsonIgnore]
        public bool SenderDeleted { get; set; }
        [JsonIgnore]
        public bool RecipientDeleted { get; set; }

    }
}

                