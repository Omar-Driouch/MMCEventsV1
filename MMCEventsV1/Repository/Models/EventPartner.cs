using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MMCEventsV1.Repository.Models;

[Table("EventPartner")]
public partial class EventPartner
{
    [Key]
    [Column("EventPartnerID")]
    public int EventPartnerId { get; set; }

    [Column("PartnerID")]
    public int? PartnerId { get; set; }

    [Column("EventID")]
    public int? EventId { get; set; }

    [ForeignKey("EventId")]
    [InverseProperty("EventPartners")]
    public virtual Event? Event { get; set; }

    [ForeignKey("PartnerId")]
    [InverseProperty("EventPartners")]
    public virtual Partner? Partner { get; set; }
}
