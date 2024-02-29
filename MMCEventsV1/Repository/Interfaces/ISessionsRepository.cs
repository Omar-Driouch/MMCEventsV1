using Microsoft.AspNetCore.Mvc;
using MMCEventsV1.DTO;
using MMCEventsV1.DTO.Session;

namespace MMCEventsV1.Repository.Interfaces
{
    public interface ISessionsRepository
    {
        Task<ActionResult<IEnumerable<SessionResponseModel>>> GetSessions();
        Task<bool> AddNewSession(SessionInputModel sessionInputModel);
        Task<ActionResult<IEnumerable<SessionResponseModel>>> GetSessionByEvent(int EventID);
        Task<bool> DeleteSession(int SessionID);
        Task<bool> UpdateSession(SessionsUpdateModel sessionsUpdateModel);
    }
}
