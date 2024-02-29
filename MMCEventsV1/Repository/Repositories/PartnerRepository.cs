using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMCEventsV1.DTO.Partners;
using MMCEventsV1.Repository.Interfaces;
using MMCEventsV1.Repository.Models;

namespace MMCEventsV1.Repository.Repositories
{
    public class PartnerRepository : IPartnerRepository
    {
        private readonly MMC_Event _PartnerContext;
        public PartnerRepository(MMC_Event partnerContext)
        {
            _PartnerContext = partnerContext;
        }


        //DONE
        public async Task<ActionResult<IEnumerable<PartnersResponseModel>>?> GetAll()
        {
            try
            {
                var partners = await _PartnerContext.Partners.ToListAsync();

                if (partners.Any())
                {
                    var result = partners.Select(item => new PartnersResponseModel
                    {
                        PartnerID = item.PartnerId,
                        Logo = item.Logo,
                        Name = item.Name,
                    }).ToList();

                    return (result);
                }
                else
                {
                    return (new List<PartnersResponseModel>()); 
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while fetching partners: " + ex.Message);
            }
        }
        //DONE
        public async Task<int> AddNewPartner(PartnersInputModel partenrInputModel)
        {
            try
            {
                Partner partner = new Partner();
                partner.Name = partenrInputModel.Name;
                partner.Logo = partenrInputModel.Logo;

                await _PartnerContext.AddAsync(partner);
                await _PartnerContext.SaveChangesAsync();

                return partner.PartnerId; 
            }
            catch (Exception ex)
            {
            
                throw new Exception("Error occurred while adding a new partner" + ex);
            }
        }
        //DONE
        public async Task<bool> UpdatedPartner(PartnersResponseModel partnerInputModel)
        {
            try
            {
                var updatedPartner = await _PartnerContext.Partners.FindAsync(partnerInputModel.PartnerID);
                if (updatedPartner != null)
                {
                    updatedPartner.PartnerId = partnerInputModel.PartnerID;
                    updatedPartner.Logo = partnerInputModel.Logo;
                    updatedPartner.Name = partnerInputModel.Name;
                    _PartnerContext.Partners.Update(updatedPartner);

                    var saved = await _PartnerContext.SaveChangesAsync();
                    return saved > 0;
                }
                else
                {
                    return false; 
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while updating the partner: " + ex.Message);
            }
        }
        //DONE
        public async Task<bool> DeletePartner(int PartnerID)
        {
            try
            {
                var updatedPartner = await _PartnerContext.Partners.FindAsync(PartnerID);
                if (updatedPartner != null)
                {
                    var EventPartner = await _PartnerContext.EventPartners.Where(Ep => Ep.PartnerId == PartnerID).ToListAsync();
                    if (updatedPartner != null)
                    {
                        _PartnerContext.EventPartners.RemoveRange(EventPartner);
                    }
                    var Partner = await _PartnerContext.Partners.FindAsync(PartnerID);
                    if(Partner != null)
                    {
                        _PartnerContext.Partners.Remove(Partner);
                    }
                    var saved = await _PartnerContext.SaveChangesAsync();
                    return saved > 0;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while updating the partner: " + ex.Message);
            }
        }
        //DONE
        public async Task<PartnersResponseModel?> GetPartnerById(int PartnerID)
        {
            try
            {
                var Partner = await _PartnerContext.Partners.FindAsync(PartnerID);
                if (Partner != null)
                {
                    PartnersResponseModel partnersResponseModel = new()
                    {
                        PartnerID = PartnerID,
                        Logo = Partner.Logo,
                        Name = Partner.Name
                    };
                    return (partnersResponseModel);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while updating the partner: " + ex.Message);
            }
        }
    }
}
