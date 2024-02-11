using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MMCEventsV1.Repository.Models;

[Index("SessionId", "UserId", Name = "UC_Session_User", IsUnique = true)]
public partial class SessionsParticipant
{
    [Key]
    [Column("ParticipateID")]
    public int ParticipateId { get; set; }

    [Column("SessionID")]
    public int? SessionId { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    [ForeignKey("SessionId")]
    [InverseProperty("SessionsParticipants")]
    public virtual Session? Session { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("SessionsParticipants")]
    public virtual User? User { get; set; }
}
