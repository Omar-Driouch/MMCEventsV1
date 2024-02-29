namespace MMCEventsV1.DTO.Events
{
    public class EventInputModel
    {
        public int? EventID { get; set; }
        public string? Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Picture { get; set; }
        public string? Description { get; set; }
    }
}
