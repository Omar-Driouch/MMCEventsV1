using Microsoft.AspNetCore.Mvc;
using MMCEventsV1.DTO;

namespace MMCEventsV1.Repository.Interfaces
{
    public interface ISessionsRepository
    {
        Task<ActionResult<IEnumerable<SessionResponseModel>>> GetSessions();
        Task<bool> AddNewSession([FromBody] SessionInputModel sessionInputModel, int EventID);
        Task<ActionResult<IEnumerable<SessionResponseModel>>> GetSessionByEvent(int EventID);

    }
}
