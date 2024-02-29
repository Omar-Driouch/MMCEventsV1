using Microsoft.Extensions.Logging;
using MMCEventsV1.Repository.Models;
using System.Net;

namespace MMCEventsV1.DTO.Session
{
    public class SessionResponseModel
    {
        public int SessionID { get; set; }
        public string? Title { get; set; }
        public DateTime? DateSession { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Picture { get; set; }

    }
}
