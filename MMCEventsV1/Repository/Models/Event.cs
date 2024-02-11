using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MMCEventsV1.Repository.Models;

[Table("Event")]
public partial class Event
{
    [Key]
    [Column("EventID")]
    public int EventId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Title { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Picture { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    [Column(TypeName = "text")]
    public string? Description { get; set; }

    [InverseProperty("Event")]
    public virtual ICollection<EventPartner> EventPartners { get; } = new List<EventPartner>();

    [InverseProperty("Event")]
    public virtual ICollection<Session> Sessions { get; } = new List<Session>();
}
