using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MMCEventsV1.Repository.Models;

[Table("SponsorSession")]
public partial class SponsorSession
{
    [Key]
    [Column("SponsorSessionID")]
    public int SponsorSessionId { get; set; }

    [Column("SponsorID")]
    public int? SponsorId { get; set; }

    [Column("SessionID")]
    public int? SessionId { get; set; }

    [ForeignKey("SessionId")]
    [InverseProperty("SponsorSessions")]
    public virtual Session? Session { get; set; }

    [ForeignKey("SponsorId")]
    [InverseProperty("SponsorSessions")]
    public virtual Sponsor? Sponsor { get; set; }
}
