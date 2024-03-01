using Microsoft.AspNetCore.Mvc;
using MMCEventsV1.DTO.Events;
using MMCEventsV1.DTO.Partners;
using MMCEventsV1.DTO.Session;
using MMCEventsV1.DTO.SessionsParticipants;
using MMCEventsV1.DTO.Speaker;
using MMCEventsV1.DTO.User;
using MMCEventsV1.Repository.Models;

namespace MMCEventsV1.Repository.Interfaces
{
    public interface ISessionsParticipants
    {
        Task<ActionResult<IEnumerable<SessionsParticipantsResponseModel>>?> GetAllSessionsParticipants();
        Task<bool> AddNewSessionsParticipants(SessionsParticipantsInputModel InputModel);
        Task<bool> UpdatedSessionsParticipants(SessionsParticipantsResponseModel InputModel);
        Task<bool> DeleteUserFromSessionByUserID(int SessionID, int UserID);
        Task<bool> DeleteSessionsParticipantsByUser(int UserID);
        Task<bool> DeleteAllUserFromSessionByUserID(int SessionID);
        Task<SessionsParticipantsResponseModel?> GetSessionsParticipantsByUser(int UserID);
        Task<ActionResult<IEnumerable<SessionParticipantsUsers>>?> GetAllSessionsParticipantsByUser(int SessionID);
        Task<ActionResult<IEnumerable<SpeakerResponseModel>>?> GetAllSessionsParticipantsBySpeaker(int SessionID);
    }
}
