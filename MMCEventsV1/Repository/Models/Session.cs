using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MMCEventsV1.Repository.Models;

public partial class Session
{
    [Key]
    [Column("SessionID")]
    public int SessionId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Title { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateSession { get; set; }

    [Column(TypeName = "text")]
    public string? Description { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Address { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Picture { get; set; }

    [Column("EventID")]
    public int? EventId { get; set; }

    [ForeignKey("EventId")]
    [InverseProperty("Sessions")]
    public virtual Event? Event { get; set; }

    [InverseProperty("Session")]
    public virtual ICollection<SessionsParticipant> SessionsParticipants { get; } = new List<SessionsParticipant>();

    [InverseProperty("Session")]
    public virtual ICollection<SponsorSession> SponsorSessions { get; } = new List<SponsorSession>();

    [InverseProperty("Session")]
    public virtual ICollection<SupportSession> SupportSessions { get; } = new List<SupportSession>();
}
