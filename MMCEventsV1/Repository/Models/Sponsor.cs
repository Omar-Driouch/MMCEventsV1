using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MMCEventsV1.Repository.Models;

[Table("Sponsor")]
public partial class Sponsor
{
    [Key]
    [Column("SponsorID")]
    public int SponsorId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Name { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Logo { get; set; }

    [InverseProperty("Sponsor")]
    public virtual ICollection<SponsorSession> SponsorSessions { get; } = new List<SponsorSession>();
}
