using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMCEventsV1.DTO;
using MMCEventsV1.Repository.Interfaces;
using MMCEventsV1.Repository.Models;

namespace MMCEventsV1.Repository.Repositories
{
    public class SessionsRepository: ISessionsRepository
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
                throw new Exception( "Internal server error: " + ex.Message); 
            }
        }

        public async Task<bool> AddNewSession([FromBody] SessionInputModel sessionInputModel, int EventID) //DONE 
        {
            try
            {
                var findEvent = await _Sessions.Events.FindAsync(EventID);
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
                    EventId = EventID
                };
                await _Sessions.Sessions.AddAsync(NewSession);
             var saved =    await _Sessions.SaveChangesAsync();
                return saved > 0;
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while adding a new session." + ex);
            }
            
        }

        public async Task<ActionResult<IEnumerable<SessionResponseModel>>> GetSessionByEvent(int EventID)
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
                throw new Exception( "An error occurred while fetching sessions by event: " + ex.Message);
            }
        }

    }
}
