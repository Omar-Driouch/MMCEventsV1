using Microsoft.AspNetCore.Mvc;
using MMCEventsV1.DTO.EventPartner;
using MMCEventsV1.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMCEventsV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventPartnersController : ControllerBase
    {
        private readonly IEventPartnerRepo _eventPartnerRepository;

        public EventPartnersController(IEventPartnerRepo eventPartnerRepository)
        {
            _eventPartnerRepository = eventPartnerRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventPartnerResponseModel>>> GetAllEventPartners()
        {
            try
            {
                var eventPartners = await _eventPartnerRepository.GetAllEventPartners();
                if (eventPartners != null)
                {
                    return Ok(eventPartners.Value);
                }
                else
                {
                    return NotFound("No event partners found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching event partners. " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNewEventPartner(EventPartnerInputModel inputModel)
        {
            try
            {
                var isAdded = await _eventPartnerRepository.AddNewEventPartner(inputModel);
                if (isAdded)
                {
                    return Ok("Event partner has been added successfully.");
                }
                else
                {
                    return BadRequest("Failed to add event partner.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding a new event partner. " + ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEventPartner(EventPartnerResponseModel inputModel)
        {
            try
            {
                var isUpdated = await _eventPartnerRepository.UpdateEventPartner(inputModel);
                if (isUpdated)
                {
                    return Ok("Event partner has been updated successfully.");
                }
                else
                {
                    return NotFound("Event partner not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating event partner. " + ex.Message);
            }
        }

        [HttpDelete("{eventPartnerId}")]
        public async Task<IActionResult> DeleteEventPartner(int eventPartnerId)
        {
            try
            {
                var isDeleted = await _eventPartnerRepository.DeleteEventPartner(eventPartnerId);
                if (isDeleted)
                {
                    return Ok("Event partner has been deleted successfully.");
                }
                else
                {
                    return NotFound("Event partner not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting event partner. " + ex.Message);
            }
        }
    }
}
