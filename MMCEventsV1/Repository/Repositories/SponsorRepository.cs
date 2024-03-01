using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMCEventsV1.DTO.Sponsor;
using MMCEventsV1.Repository.Interfaces;
using MMCEventsV1.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMCEventsV1.Repository.Repositories
{
    public class SponsorRepository : ISponsorRepository
    {
        private readonly MMC_Event _sponsorContext;

        public SponsorRepository(MMC_Event sponsorContext)
        {
            _sponsorContext = sponsorContext;
        }
        // VERIFIED 
        public async Task<ActionResult<IEnumerable<SponsorResponseModel>>?> GetAll()
        {
            try
            {
                var sponsors = await _sponsorContext.Sponsors.ToListAsync();

                if (sponsors.Any())
                {
                    var result = sponsors.Select(item => new SponsorResponseModel
                    {
                        SponsorId = item.SponsorId,
                        Name = item.Name,
                        Logo = item.Logo,
                        // Add other properties as needed
                    }).ToList();

                    return result;
                }
                else
                {
                    return new List<SponsorResponseModel>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while fetching sponsors: " + ex.Message);
            }
        }
        // VERIFIED 
        public async Task<int> AddNewSponsor(SponsorInputModel sponsorInputModel)
        {
            try
            {
                var sponsor = new Sponsor
                {
                    Name = sponsorInputModel.Name,
                    Logo = sponsorInputModel.Logo,
                    // Set other properties as needed
                };

                await _sponsorContext.AddAsync(sponsor);
                await _sponsorContext.SaveChangesAsync();

                return sponsor.SponsorId;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding a new sponsor: " + ex.Message);
            }
        }
        // VERIFIED 
        public async Task<bool> UpdatedSponsor(SponsorResponseModel sponsorInputModel)
        {
            try
            {
                var updatedSponsor = await _sponsorContext.Sponsors.FindAsync(sponsorInputModel.SponsorId);
                if (updatedSponsor != null)
                {
                    updatedSponsor.Name = sponsorInputModel.Name;
                    updatedSponsor.Logo = sponsorInputModel.Logo;
                    // Update other properties as needed
                    _sponsorContext.Sponsors.Update(updatedSponsor);

                    var saved = await _sponsorContext.SaveChangesAsync();
                    return saved > 0;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while updating the sponsor: " + ex.Message);
            }
        }
        // VERIFIED 
        public async Task<bool> DeleteSponsor(int SponsorID)
        {
            try
            {
                var sponsor = await _sponsorContext.Sponsors.FindAsync(SponsorID);
                if (sponsor != null)
                {
                    var sponsorSession = await _sponsorContext.SponsorSessions.Where(sp=>sp.SponsorId==SponsorID).ToListAsync();
                    if (sponsorSession != null) _sponsorContext.SponsorSessions.RemoveRange(sponsorSession);
                    _sponsorContext.Sponsors.Remove(sponsor);
                    var saved = await _sponsorContext.SaveChangesAsync();
                    return saved > 0;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while deleting the sponsor: " + ex.Message);
            }
        }
        // VERIFIED 
        public async Task<SponsorResponseModel?> GetSponsorById(int SponsorID)
        {
            try
            {
                var sponsor = await _sponsorContext.Sponsors.FindAsync(SponsorID);
                if (sponsor != null)
                {
                    var sponsorResponseModel = new SponsorResponseModel
                    {
                        SponsorId = sponsor.SponsorId,
                        Name = sponsor.Name,
                        Logo = sponsor.Logo,
                        // Set other properties as needed
                    };
                    return sponsorResponseModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while getting the sponsor: " + ex.Message);
            }
        }
    }
}
