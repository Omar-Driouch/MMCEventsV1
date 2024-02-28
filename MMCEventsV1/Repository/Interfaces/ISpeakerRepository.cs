using Microsoft.AspNetCore.Mvc;
using MMCEventsV1.DTO;
using MMCEventsV1.DTO.Speaker;
using MMCEventsV1.DTO.User;
using MMCEventsV1.Repository.Models;

namespace MMCEventsV1.Repository.Interfaces
{
    public interface ISpeakerRepository
    {

        Task<ActionResult<ICollection<SpeakerResponseModel>>> GetSpeakers();
        Task<bool> AddSpeaker([FromBody] SpeakerInputModel inputModel);
        Task<IActionResult> UpdateSpeaker([FromBody] SpeakerInputModel inputModel);
        Task<ActionResult<SpeakerResponseModel>> GetSpeakerByID(int SpeakerID);
        Task<ActionResult<Speaker>> DeleteSpeaker(int SpeakerID);
        Task<bool> SaveAsync();


    }
}
