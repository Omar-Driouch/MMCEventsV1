using Microsoft.AspNetCore.Mvc;
using MMCEventsV1.DTO.Events;
using MMCEventsV1.DTO.Partners;
using MMCEventsV1.DTO.Session;
using MMCEventsV1.DTO.SessionsParticipants;
using MMCEventsV1.DTO.Speaker;
using MMCEventsV1.DTO.SupportSession;
using MMCEventsV1.DTO.User;
using MMCEventsV1.Repository.Models;

namespace MMCEventsV1.Repository.Interfaces
{
    public interface ISessionsSupportRepo
    {
        Task<ActionResult<IEnumerable<SupportSessionResponseModel>>?> GetAllSessionsSupports();
        
        
        Task<bool> AddNewSessionsSupport(SupportSessionInputModel InputModel);
        Task<bool> UpdatedSessionsSupport(SupportSessionResponseModel InputModel);
        Task<bool> DeleteSupportSessionBySessionID(int SessionID);
        Task<bool> DeleteSessionSupportsBySupportID(int SessionID);


        //Task<bool> DeleteSessionsParticipantsByUser(int UserID);
        //Task<bool> DeleteAllUserFromSessionByUserID(int SessionID);
        //Task<SessionsParticipantsResponseModel?> GetSessionsParticipantsByUser(int UserID);
        //Task<ActionResult<IEnumerable<SessionParticipantsUsers>>?> GetAllSessionsParticipantsByUser(int SessionID);
       
        Task<ActionResult<IEnumerable<SupportSessionResponseModel>>?> GetAllSessionSupportsBySupportID(int SupportID);
        Task<ActionResult<IEnumerable<SupportSessionResponseModel>>?> GetAllSessionSupportsBySessionID(int SessionID);
    }
}
