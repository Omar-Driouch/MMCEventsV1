using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MMCEventsV1.Repository.Models;

public partial class SocialMedia
{
    [Key]
    [Column("SpeakerID")]
    public int SpeakerId { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Facebook { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Instagram { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? LinkedIn { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Twitter { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Website { get; set; }

    [ForeignKey("SpeakerId")]
    [InverseProperty("SocialMedia")]
    public virtual Speaker Speaker { get; set; } = null!;


}
