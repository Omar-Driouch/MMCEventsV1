using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MMCEventsV1.DTO.Support
{
    public class SupportInputModel
    {
       
        //public int SupportId { get; set; }
        public string? Name { get; set; }
        public string? Path { get; set; }
        public string? Status { get; set; }
        public int? Duration { get; set; }

        
    }
}
