using MMCEventsV1.Repository.Models;

namespace MMCEventsV1.DTO
{
    public class EventUpdateModel
    {
        
        public  string ?Title { get; set; }
        public  DateTime ?StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ?Picture { get; set; }
        public string ?Description { get; set; }
    }

}
