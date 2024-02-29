namespace MMCEventsV1.DTO.Session
{
    public class SessionInputModel
    {
        public int EventID { get; set; }
        public string? Title { get; set; }
        public DateTime? DateSession { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Picture { get; set; }
    }
}
