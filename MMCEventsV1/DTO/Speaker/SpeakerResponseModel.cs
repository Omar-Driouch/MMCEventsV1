using MMCEventsV1.Repository.Models;

namespace MMCEventsV1.DTO.Speaker
{
    public class SpeakerResponseModel
    {

        public int? SpeakerID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SpeakerEmail { get; set; }
        public string? Phone { get; set; }
        public string? City { get; set; }
        public string? Gender { get; set; }
        public string? Picture { get; set; }
        public bool? Mct { get; set; }
        public bool? Mvp { get; set; }
        public string? Biography { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? LinkedIn { get; set; }
        public string? Twitter { get; set; }
        public string? Website { get; set; }
    }
}
