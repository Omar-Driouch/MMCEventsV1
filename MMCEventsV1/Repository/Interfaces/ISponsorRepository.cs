using Microsoft.AspNetCore.Mvc;
using MMCEventsV1.DTO.Events;
using MMCEventsV1.DTO.Partners;
using MMCEventsV1.DTO.Session;
using MMCEventsV1.DTO.Sponsor;
using MMCEventsV1.Repository.Models;

namespace MMCEventsV1.Repository.Interfaces
{
    public interface ISponsorRepository
    {
        Task<ActionResult<IEnumerable<SponsorResponseModel>>?> GetAll();
        Task<int> AddNewSponsor(SponsorInputModel partenrInputModel);

        Task<bool> UpdatedSponsor(SponsorResponseModel partenrInputModel);

        Task<bool> DeleteSponsor(int PartnerID);
        Task<SponsorResponseModel?> GetSponsorById(int PartnerID);
    }
}
