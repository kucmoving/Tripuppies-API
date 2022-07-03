namespace API.Models
{
    public class TravelRecord
    {
        public int Id { get; set; } 
        public DateTime TravelYear { get; set; }    
        public String Summary { get; set; }
        public string VisitRegion { get; set; }
        public AppUser AppUser { get; set; }    
        public int AppUserId { get; set; }
    }
}
