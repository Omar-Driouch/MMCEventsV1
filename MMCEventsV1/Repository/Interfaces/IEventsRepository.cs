using Microsoft.AspNetCore.Mvc;
using MMCEventsV1.DTO.Events;

namespace MMCEventsV1.Repository.Interfaces
{
    public interface IEventsRepository
    {
        Task<ActionResult<IEnumerable<EventInputModel>>> GetEvents();
        Task<ActionResult<EventResponseModel>> GetEventById(int EventID);
        Task<bool> CreateEvent(EventUpdateModel eventUpdateModel);
        Task<bool> UpdateEvent( EventInputModel inputModel);

        Task<bool> DeleteEvent(int EventID);
    }
}
