using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MMCEventsV1.Repository.Models;

public partial class Partner
{
    [Key]
    [Column("PartnerID")]
    public int PartnerId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Name { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Logo { get; set; }

    [InverseProperty("Partner")]
    public virtual ICollection<EventPartner> EventPartners { get; } = new List<EventPartner>();
}
