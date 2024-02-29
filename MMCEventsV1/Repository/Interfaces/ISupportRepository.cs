using Microsoft.AspNetCore.Mvc;
using MMCEventsV1.DTO.Partners;
using MMCEventsV1.DTO.Support;

namespace MMCEventsV1.Repository.Interfaces
{
    public interface ISupportRepository
    {
        Task<ActionResult<IEnumerable<SupportResponseModel>>?> GetAll();
        Task<bool> AddNewSupport(SupportInputModel partenrInputModel);

        Task<bool> UpdateSupport(SupportResponseModel partenrInputModel);

        Task<bool> DeleteSupport(int PartnerID);
        Task<SupportResponseModel?> GetSupportById(int PartnerID);

    }
}
