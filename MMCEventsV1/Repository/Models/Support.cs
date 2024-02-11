using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MMCEventsV1.Repository.Models;

[Table("Support")]
public partial class Support
{
    [Key]
    [Column("SupportID")]
    public int SupportId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Name { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Path { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? Status { get; set; }

    [Column("duration")]
    public int? Duration { get; set; }

    [InverseProperty("Support")]
    public virtual ICollection<SupportSession> SupportSessions { get; } = new List<SupportSession>();
}
