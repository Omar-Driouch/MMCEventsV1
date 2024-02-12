using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MMCEventsV1.Repository.Models;

[Index("UserEmail", Name = "UQ__Users__08638DF80A2DB669", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("UserID")]
    public int UserId { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string UserEmail { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string UserPassword { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? FirstName { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? LastName { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Gender { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? City { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string UserStatus { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<SessionsParticipant> SessionsParticipants { get; } = new List<SessionsParticipant>();

    //[InverseProperty("SpeakerNavigation")]
    //public virtual Speaker? Speaker { get; set; }
}
