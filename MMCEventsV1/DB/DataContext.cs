using System;
using System.Collections.Generic;
using MMCEventsV1.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace MMCEventsV1.Repository;

public partial class MMC_Event : DbContext
{
    public MMC_Event()
    {
    }

    public MMC_Event(DbContextOptions<MMC_Event> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventPartner> EventPartners { get; set; }

    public virtual DbSet<Partner> Partners { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<SessionsParticipant> SessionsParticipants { get; set; }

    public virtual DbSet<SocialMedia> SocialMedia { get; set; }

    public virtual DbSet<Speaker> Speakers { get; set; }

    public virtual DbSet<Sponsor> Sponsors { get; set; }

    public virtual DbSet<SponsorSession> SponsorSessions { get; set; }

    public virtual DbSet<Support> Supports { get; set; }

    public virtual DbSet<SupportSession> SupportSessions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=OMARPC\\LOS;Initial Catalog=MMC_Event;Integrated Security=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Event__7944C8701BC86C94");

            entity.ToTable("Event", tb => tb.HasTrigger("trg_DeleteEvent"));
        });

        modelBuilder.Entity<EventPartner>(entity =>
        {
            entity.HasKey(e => e.EventPartnerId).HasName("PK__EventPar__941C85A56B8B0E84");

            entity.HasOne(d => d.Event).WithMany(p => p.EventPartners).HasConstraintName("FK__EventPart__Event__5441852A");

            entity.HasOne(d => d.Partner).WithMany(p => p.EventPartners).HasConstraintName("FK__EventPart__Partn__534D60F1");
        });

        modelBuilder.Entity<Partner>(entity =>
        {
            entity.HasKey(e => e.PartnerId).HasName("PK__Partners__39FD6332D3A277C8");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__Sessions__C9F492703981ADCF");

            entity.HasOne(d => d.Event).WithMany(p => p.Sessions).HasConstraintName("FK__Sessions__EventI__49C3F6B7");
        });

        modelBuilder.Entity<SessionsParticipant>(entity =>
        {
            entity.HasKey(e => e.ParticipateId).HasName("PK__Sessions__07F73A76B4C44C61");

            entity.HasOne(d => d.Session).WithMany(p => p.SessionsParticipants)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__SessionsP__Sessi__4D94879B");

            entity.HasOne(d => d.User).WithMany(p => p.SessionsParticipants).HasConstraintName("FK__SessionsP__UserI__4E88ABD4");
        });

        modelBuilder.Entity<SocialMedia>(entity =>
        {
            entity.HasKey(e => e.SpeakerId).HasName("PK__SocialMe__79E757394196DA68");

            entity.Property(e => e.SpeakerId).ValueGeneratedNever();

            entity.HasOne(d => d.Speaker).WithOne(p => p.SocialMedia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SocialMed__Speak__4316F928");
        });

        modelBuilder.Entity<Speaker>(entity =>
        {
            entity.HasKey(e => e.SpeakerId).HasName("PK__Speakers__79E757397AB74278");

            entity.ToTable(tb =>
            {
                tb.HasTrigger("deleteSocialMediaRowOnDeleleSpeaker");
                tb.HasTrigger("trg_CreateSpeaker");
                tb.HasTrigger("trg_DeleteSocialMSpeaker");
                tb.HasTrigger("trg_DeleteSpeaker");
            });

            entity.Property(e => e.SpeakerId).ValueGeneratedNever();

        //    entity.HasOne(d => d.SpeakerNavigation).WithOne(p => p.Speaker)
        //        .OnDelete(DeleteBehavior.ClientSetNull)
        //        .HasConstraintName("FK__Speakers__Speake__3E52440B");
        });

        modelBuilder.Entity<Sponsor>(entity =>
        {
            entity.HasKey(e => e.SponsorId).HasName("PK__Sponsor__3B609EF576CD258A");
        });

        modelBuilder.Entity<SponsorSession>(entity =>
        {
            entity.HasKey(e => e.SponsorSessionId).HasName("PK__SponsorS__B265122B923B7531");

            entity.HasOne(d => d.Session).WithMany(p => p.SponsorSessions).HasConstraintName("FK__SponsorSe__Sessi__59FA5E80");

            entity.HasOne(d => d.Sponsor).WithMany(p => p.SponsorSessions).HasConstraintName("FK__SponsorSe__Spons__59063A47");
        });

        modelBuilder.Entity<Support>(entity =>
        {
            entity.HasKey(e => e.SupportId).HasName("PK__Support__D82DBC6CCB8FCE4F");
        });

        modelBuilder.Entity<SupportSession>(entity =>
        {
            entity.HasKey(e => e.SessionSponsorId).HasName("PK__SupportS__213DA127A6C22E02");

            entity.HasOne(d => d.Session).WithMany(p => p.SupportSessions).HasConstraintName("FK__SupportSe__Sessi__5FB337D6");

            entity.HasOne(d => d.Support).WithMany(p => p.SupportSessions).HasConstraintName("FK__SupportSe__Suppo__5EBF139D");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC337E3A65");

            entity.ToTable(tb =>
            {
                tb.HasTrigger("deleteUserTrigger");
                tb.HasTrigger("trg_DeleteUser");
            });

            entity.Property(e => e.UserStatus).HasDefaultValueSql("('User')");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
