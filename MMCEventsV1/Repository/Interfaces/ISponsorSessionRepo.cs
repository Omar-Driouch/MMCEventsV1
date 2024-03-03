using Microsoft.AspNetCore.Mvc;
using MMCEventsV1.DTO.SponsorSessions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMCEventsV1.Repository.Interfaces
{
    public interface ISponsorSessionRepo
    {
        Task<ActionResult<IEnumerable<SponsorSessionsResponseModel>>> GetAllSponsorSessions();
        Task<bool> AddNewSponsorSession(SponsorSessionsInputModel inputModel);
        Task<bool> UpdateSponsorSession(SponsorSessionsResponseModel inputModel);
        Task<bool> DeleteSponsorSession(int sessionSponsorId);
    }
}
