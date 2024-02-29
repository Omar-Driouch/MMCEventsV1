using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMCEventsV1.DTO.Events;
using MMCEventsV1.DTO.Partners;
using MMCEventsV1.Repository.Interfaces;
using MMCEventsV1.Repository.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MMCEventsV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnerController : ControllerBase
    {
        private readonly IPartnerRepository _partnerRepository;
        public PartnerController(IPartnerRepository partnerRepository)
        {
            _partnerRepository = partnerRepository;
        }

        //VERIFIED 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PartnersResponseModel>>> GetPartner()
        {
            try
            {
                var partners = await _partnerRepository.GetAll();
                if (partners != null)
                {
                    return Ok(partners.Value);
                }
                else
                {
                    return NotFound("No partners found.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or return a proper error response
                throw new Exception("An error occurred while fetching partners: " + ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddNewPartner(PartnersInputModel partenrInputModel)
        {
            try
            {
                int partnerId = await _partnerRepository.AddNewPartner(partenrInputModel);
                return Ok($"Partner with ID {partnerId} added successfully");
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while adding a new partner. Please try again later." + ex);
            }
        }
        //VERIFIED 
        [HttpPut("{PartnerId}")]
        public async Task<IActionResult> UpdatePartner(PartnersResponseModel partenrInputModel)
        {
            try
            {
                var partnerId = await _partnerRepository.UpdatedPartner(partenrInputModel);
                if (partnerId)
                {
                    return Ok($"Partner with ID {partenrInputModel.PartnerID} Updated successfully");
                }
                else
                {
                    return NotFound("Partner Not Found.");
                }
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while Updating partner. Please try again later." + ex);
            }
        }

        //VERIFIED 
        [HttpDelete("{PartnerID}")]
        public async Task<IActionResult> DeletePartner(int PartnerID)
        {
            try
            {
                var partnerId = await _partnerRepository.DeletePartner(PartnerID);
                if (partnerId)
                {
                    return Ok($"Partner with ID {PartnerID} has been deleted successfully");
                }
                else
                {
                    return NotFound("Partner Not Found.");
                }
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while deleting partner. Please try again later." + ex);
            }
        }

        //VERIFIED 
        [HttpGet("{PartnerID}")]
        public async Task<IActionResult> GetPartnerById(int PartnerID)
        {
            try
            {
                var partnerId = await _partnerRepository.GetPartnerById(PartnerID);
                if (partnerId != null)
                {
                    return Ok(partnerId);
                }
                else
                {
                    return NotFound("Partner Not Found.");
                }
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while fetching partner. Please try again later." + ex);
            }
        }
    }
}
