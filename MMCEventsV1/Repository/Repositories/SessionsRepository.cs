using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMCEventsV1.DTO.Session;
using MMCEventsV1.Repository.Interfaces;
using MMCEventsV1.Repository.Models;

namespace MMCEventsV1.Repository.Repositories
{
    public class SessionsRepository : ISessionsRepository
    {
        public readonly MMC_Event _Sessions;
        public SessionsRepository(MMC_Event context)
        {
            _Sessions = context;
        }


        public async Task<ActionResult<IEnumerable<SessionResponseModel>>> GetSessions()// DONE
        {
            try
            {
                var sessions = await _Sessions.Sessions.ToListAsync();

                if (sessions == null || !sessions.Any())
                {
                    return null;
                }

                var responseModels = sessions.Select(session => new SessionResponseModel
                {
                    SessionID = session.SessionId,
                    Title = session.Title,
                    DateSession = session.DateSession,
                    Description = session.Description,
                    Address = session.Address,
                    Picture = session.Picture
                }).ToList();

                return (responseModels);
            }
            catch (Exception ex)
            {
                throw new Exception("Internal server error: " + ex.Message);
            }
        }

        public async Task<bool> AddNewSession([FromBody] SessionInputModel sessionInputModel) //DONE 
        {
            try
            {
                var findEvent = await _Sessions.Events.FindAsync(sessionInputModel.EventID);
                if (findEvent == null)
                {
                    return false;
                }
                Session NewSession = new()
                {
                    Title = sessionInputModel.Title,
                    DateSession = sessionInputModel.DateSession,
                    Address = sessionInputModel.Address,
                    Picture = sessionInputModel.Picture,
                    Description = sessionInputModel.Description,
                    EventId = sessionInputModel.EventID
                };
                await _Sessions.Sessions.AddAsync(NewSession);
                var saved = await _Sessions.SaveChangesAsync();
                return saved > 0;
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while adding a new session." + ex);
            }

        }

        public async Task<ActionResult<IEnumerable<SessionResponseModel>>> GetSessionByEvent(int EventID) //DONE 
        {
            try
            {
                var IsEventID = await _Sessions.Events.FindAsync(EventID);
                if (IsEventID != null)
                {
                    var allSessions = await _Sessions.Sessions.Where(session => session.EventId == EventID).ToListAsync();
                    List<SessionResponseModel> allSessionsByEvent = new List<SessionResponseModel>();
                    foreach (var item in allSessions)
                    {
                        SessionResponseModel session = new SessionResponseModel
                        {
                            SessionID = item.SessionId,
                            Title = item.Title,
                            DateSession = item.DateSession,
                            Address = item.Address,
                            Picture = item.Picture,
                            Description = item.Description
                        };
                        allSessionsByEvent.Add(session);
                    }
                    return (allSessionsByEvent);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching sessions by event: " + ex.Message);
            }
        }


        public async Task<bool> DeleteSession(int SessionID)
        {
            try
            {
                var Deleted = await _Sessions.Sessions.FindAsync(SessionID);
                if (Deleted == null)
                {
                    return false;
                }
                var SupportSessionsToDelete = await _Sessions.SupportSessions.Where(es => es.SessionId == SessionID).ToListAsync();
                if (SupportSessionsToDelete.Any())
                {
                    _Sessions.SupportSessions.RemoveRange(SupportSessionsToDelete);
                    await _Sessions.SaveChangesAsync();
                }
                var SponsorSessionsToDelete = await _Sessions.SponsorSessions.Where(es => es.SessionId == SessionID).ToListAsync();
                if (SponsorSessionsToDelete.Any())
                {
                    _Sessions.SponsorSessions.RemoveRange(SponsorSessionsToDelete);
                    await _Sessions.SaveChangesAsync();
                }

                _Sessions.Remove(Deleted);
                var saved = await _Sessions.SaveChangesAsync();
                return saved > 0;

            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching sessions by event: " + ex.Message);
            }

        }

        public async Task<bool> UpdateSession(SessionsUpdateModel sessionsUpdateModel)
        {
            try
            {
                var sessionToUpdate = await _Sessions.Sessions.FindAsync(sessionsUpdateModel.SessionID);
                if (sessionToUpdate != null)
                {
                    sessionToUpdate.Picture = sessionsUpdateModel.Picture;
                    sessionToUpdate.Title = sessionsUpdateModel.Title;
                    sessionToUpdate.Address = sessionsUpdateModel.Address;
                    sessionToUpdate.DateSession = sessionsUpdateModel.DateSession;
                    sessionToUpdate.Description = sessionsUpdateModel.Description;

                    var saved = await _Sessions.SaveChangesAsync();
                    return saved > 0;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Error occurred while updating session: " + ex.Message);
            }
        }

    }
}
