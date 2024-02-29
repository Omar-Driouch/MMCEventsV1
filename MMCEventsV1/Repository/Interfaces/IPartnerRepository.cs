using Microsoft.AspNetCore.Mvc;
using MMCEventsV1.DTO.Events;
using MMCEventsV1.DTO.Partners;
using MMCEventsV1.DTO.Session;
using MMCEventsV1.Repository.Models;

namespace MMCEventsV1.Repository.Interfaces
{
    public interface IPartnerRepository
    {
        Task<ActionResult<IEnumerable<PartnersResponseModel>>?> GetAll();
        Task<int> AddNewPartner(PartnersInputModel partenrInputModel);

        Task<bool> UpdatedPartner(PartnersResponseModel partenrInputModel);

        Task<bool> DeletePartner(int PartnerID);
        Task<PartnersResponseModel?> GetPartnerById(int PartnerID);
    }
}
