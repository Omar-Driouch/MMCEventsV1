using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MMCEventsV1.Repository.Models;

[Table("SupportSession")]
public partial class SupportSession
{
    [Key]
    [Column("sessionSponsorID")]
    public int SessionSponsorId { get; set; }

    [Column("SupportID")]
    public int? SupportId { get; set; }

    [Column("SessionID")]
    public int? SessionId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateSupportSession { get; set; }

    [ForeignKey("SessionId")]
    [InverseProperty("SupportSessions")]
    public virtual Session? Session { get; set; }

    [ForeignKey("SupportId")]
    [InverseProperty("SupportSessions")]
    public virtual Support? Support { get; set; }
}
