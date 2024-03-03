using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMCEventsV1.DTO.Session;
using MMCEventsV1.DTO.SessionsParticipants;
using MMCEventsV1.DTO.Speaker;
using MMCEventsV1.DTO.SupportSession;
using MMCEventsV1.DTO.User;
using MMCEventsV1.Repository.Interfaces;
using MMCEventsV1.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMCEventsV1.Repository.Repositories
{
    public class SessionsSupportRepository : ISessionsSupportRepo
    {
        private readonly MMC_Event _context;

        public SessionsSupportRepository(MMC_Event context)
        {
            _context = context;
        }
        //DONE
        public async Task<ActionResult<IEnumerable<SupportSessionResponseModel>>> GetAllSessionsSupports()
        {
            try
            {
                var supportSessions = await _context.SupportSessions.ToListAsync();

                var result = supportSessions.Select(item => new SupportSessionResponseModel
                {
                    sessionSponsorID = item.SessionSponsorId,
                    SupportID = item.SupportId ?? 0,
                    SessionID = item.SessionId ?? 0,
                    DateSupportSession = item.DateSupportSession ?? DateTime.Now
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while fetching session supports: " + ex.Message);
            }
        }
         
        //DONE
        public async Task<bool> AddNewSessionsSupport(SupportSessionInputModel InputModel)
        {
            try
            {
                var sessionSupport = new SupportSession
                {
                    SessionId = InputModel.SessionID,
                    SupportId = InputModel.SupportID,
                    DateSupportSession = InputModel.DateSupportSession,
                };

                await _context.SupportSessions.AddAsync(sessionSupport);
               var saved =  await _context.SaveChangesAsync();

                return saved > 0;
            }
            catch (Exception ex)
            {

                throw new Exception("Error occurred while adding a new session Support: " + ex.Message);
            }
        }
        //DONE
        public async Task<bool> UpdatedSessionsSupport(SupportSessionResponseModel InputModel)
        {
            try
            {
                var updatedParticipant = await _context.SupportSessions.FindAsync(InputModel.sessionSponsorID);
                if (updatedParticipant != null)
                {
                    updatedParticipant.SessionId = InputModel.SessionID;
                    updatedParticipant.SupportId = InputModel.SupportID;
                    updatedParticipant.DateSupportSession = InputModel.DateSupportSession;
                    _context.SupportSessions.Update(updatedParticipant);

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
                throw new Exception("Error occurred while updating the session Support: " + ex.Message);
            }
        }
        //DONE
        public async Task<bool> DeleteSupportSessionBySessionID(int SessionID)
        {
            try
            {
                var SessionSupports = await _context.SupportSessions.Where(Sp => Sp.SessionId == SessionID).ToListAsync();
                if (SessionSupports.Any())
                {
                    _context.SupportSessions.RemoveRange(SessionSupports);
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
                throw new Exception("Error occurred while deleting the SessionSupports by ID: " + ex.Message);
            }
        }
        //DONE

        public async Task<bool> DeleteSessionSupportsBySupportID(int SupportID)
        {
            try
            {
                var SessionSupports = await _context.SupportSessions.Where(Sp => Sp.SupportId == SupportID).ToListAsync();
                if (SessionSupports.Any())
                {
                    _context.SupportSessions.RemoveRange(SessionSupports);
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
                throw new Exception("Error occurred while deleting the SessionSupports by ID: " + ex.Message);
            }
        }
        //DONE
        public async Task<ActionResult<IEnumerable<SupportSessionResponseModel>>?> GetAllSessionSupportsBySupportID(int SupportID)
        {
            try
            {
                var sessionSupport = await _context.SupportSessions
                    .Where(sp => sp.SupportId == SupportID)
                    .ToListAsync();

                if (sessionSupport.Any())
                {
                    var sessionSupports = sessionSupport.Select(sp => new SupportSessionResponseModel
                    {
                        sessionSponsorID = sp.SessionSponsorId,
                        SupportID = sp.SupportId ?? 0,
                        SessionID = sp.SessionId ?? 0,
                        DateSupportSession = sp.DateSupportSession ?? DateTime.Now
                    }).ToList();

                    return sessionSupports;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception( "An error occurred while fetching session supports by SupportID: " + ex.Message);
            }
        }
        //DONE
        public async Task<ActionResult<IEnumerable<SupportSessionResponseModel>>?> GetAllSessionSupportsBySessionID(int SessionID)
        {
            try
            {
                var sessionSupport = await _context.SupportSessions
                    .Where(sp => sp.SessionId == SessionID)
                    .ToListAsync();

                if (sessionSupport.Any())
                {
                    var sessionSupports = sessionSupport.Select(sp => new SupportSessionResponseModel
                    {
                        sessionSponsorID = sp.SessionSponsorId,
                        SupportID = sp.SupportId ?? 0,
                        SessionID = sp.SessionId ?? 0,
                        DateSupportSession = sp.DateSupportSession ?? DateTime.Now
                    }).ToList();

                    return sessionSupports;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching session supports by SessionID: " + ex.Message);
            }
        }



 
    }
}
