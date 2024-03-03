using Microsoft.AspNetCore.Mvc;
using MMCEventsV1.DTO.EventPartner;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMCEventsV1.Repository.Interfaces
{
    public interface IEventPartnerRepo
    {
        Task<ActionResult<IEnumerable<EventPartnerResponseModel>>> GetAllEventPartners();
        Task<bool> AddNewEventPartner(EventPartnerInputModel inputModel);
        Task<bool> UpdateEventPartner(EventPartnerResponseModel inputModel);
        Task<bool> DeleteEventPartner(int eventPartnerId);
    }
}
