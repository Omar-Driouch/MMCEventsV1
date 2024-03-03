using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMCEventsV1.DTO.SponsorSessions;
using MMCEventsV1.Repository.Interfaces;
using MMCEventsV1.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMCEventsV1.Repository.Repositories
{
    public class SponsorSessionRepository : ISponsorSessionRepo
    {
        private readonly MMC_Event _context;

        public SponsorSessionRepository(MMC_Event context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<SponsorSessionsResponseModel>>> GetAllSponsorSessions()
        {
            try
            {
                var sponsorSessions = await _context.SponsorSessions.ToListAsync();

                var result = sponsorSessions.Select(item => new SponsorSessionsResponseModel
                {
                    SessionSponsorId = item.SponsorSessionId,
                    SponsorId = item.SponsorId ?? 0,
                    SessionId = item.SessionId ?? 0
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while fetching sponsor sessions: " + ex.Message);
            }
        }

        public async Task<bool> AddNewSponsorSession(SponsorSessionsInputModel inputModel)
        {
            try
            {
                var sponsorSession = new SponsorSession
                {
                    SponsorId = inputModel.SponsorId,
                    SessionId = inputModel.SessionId
                };

                await _context.SponsorSessions.AddAsync(sponsorSession);
                var saved = await _context.SaveChangesAsync();

                return saved > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding a new sponsor session: " + ex.Message);
            }
        }

        public async Task<bool> UpdateSponsorSession(SponsorSessionsResponseModel inputModel)
        {
            try
            {
                var updatedSponsorSession = await _context.SponsorSessions.FindAsync(inputModel.SessionSponsorId);
                if (updatedSponsorSession != null)
                {
                    updatedSponsorSession.SponsorId = inputModel.SponsorId;
                    updatedSponsorSession.SessionId = inputModel.SessionId;

                    _context.SponsorSessions.Update(updatedSponsorSession);

                    var saved = await _context.SaveChangesAsync();
                    return saved > 0;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while updating the sponsor session: " + ex.Message);
            }
        }

        public async Task<bool> DeleteSponsorSession(int sessionSponsorId)
        {
            try
            {
                var sponsorSession = await _context.SponsorSessions.FindAsync(sessionSponsorId);
                if (sponsorSession != null)
                {
                    _context.SponsorSessions.Remove(sponsorSession);
                    var deleted = await _context.SaveChangesAsync();
                    return deleted > 0;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while deleting the sponsor session: " + ex.Message);
            }
        }
    }
}
