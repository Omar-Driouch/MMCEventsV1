using Microsoft.AspNetCore.Mvc;
using MMCEventsV1.DTO.SponsorSessions;
using MMCEventsV1.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMCEventsV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SponsorSessionsController : ControllerBase
    {
        private readonly ISponsorSessionRepo _sponsorSessionRepository;

        public SponsorSessionsController(ISponsorSessionRepo sponsorSessionRepository)
        {
            _sponsorSessionRepository = sponsorSessionRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SponsorSessionsResponseModel>>> GetAllSponsorSessions()
        {
            try
            {
                var sponsorSessions = await _sponsorSessionRepository.GetAllSponsorSessions();
                if (sponsorSessions != null)
                {
                    return Ok(sponsorSessions.Value);
                }
                else
                {
                    return NotFound("No sponsor sessions found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching sponsor sessions. " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNewSponsorSession(SponsorSessionsInputModel inputModel)
        {
            try
            {
                var isAdded = await _sponsorSessionRepository.AddNewSponsorSession(inputModel);
                if (isAdded)
                {
                    return Ok("Sponsor session has been added successfully.");
                }
                else
                {
                    return BadRequest("Failed to add sponsor session.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding a new sponsor session. " + ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSponsorSession(SponsorSessionsResponseModel inputModel)
        {
            try
            {
                var isUpdated = await _sponsorSessionRepository.UpdateSponsorSession(inputModel);
                if (isUpdated)
                {
                    return Ok("Sponsor session has been updated successfully.");
                }
                else
                {
                    return NotFound("Sponsor session not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating sponsor session. " + ex.Message);
            }
        }

        [HttpDelete("{sessionSponsorId}")]
        public async Task<IActionResult> DeleteSponsorSession(int sessionSponsorId)
        {
            try
            {
                var isDeleted = await _sponsorSessionRepository.DeleteSponsorSession(sessionSponsorId);
                if (isDeleted)
                {
                    return Ok("Sponsor session has been deleted successfully.");
                }
                else
                {
                    return NotFound("Sponsor session not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting sponsor session. " + ex.Message);
            }
        }
    }
}
