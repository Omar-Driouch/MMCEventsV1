using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MMCEventsV1.Repository.Models;

public partial class Speaker
{
    [Key]
    [Column("SpeakerID")]
    public int SpeakerId { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Picture { get; set; }

    [Column("MCT")]
    public bool? Mct { get; set; }

    [Column("MVP")]
    public bool? Mvp { get; set; }

    [Column(TypeName = "text")]
    public string? Biography { get; set; }

    [InverseProperty("Speaker")]
    public virtual SocialMedia? SocialMedia { get; set; }

    //[ForeignKey("SpeakerId")]
    //[InverseProperty("Speaker")]
    //public virtual User SpeakerNavigation { get; set; } = null!;


}
