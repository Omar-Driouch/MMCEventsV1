using Microsoft.AspNetCore.Mvc;
using MMCEventsV1.DTO.Sponsor;
using MMCEventsV1.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMCEventsV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SponsorController : ControllerBase
    {
        private readonly ISponsorRepository _sponsorRepository;

        public SponsorController(ISponsorRepository sponsorRepository)
        {
            _sponsorRepository = sponsorRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SponsorResponseModel>>> GetSponsors()
        {
            try
            {
                var sponsors = await _sponsorRepository.GetAll();
                if (sponsors != null)
                {
                    return Ok(sponsors.Value);
                }
                else
                {
                    return NotFound("No sponsors found.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or return a proper error response
                throw new Exception("An error occurred while fetching sponsors: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNewSponsor(SponsorInputModel sponsorInputModel)
        {
            try
            {
                int sponsorId = await _sponsorRepository.AddNewSponsor(sponsorInputModel);
                return Ok($"Sponsor with ID {sponsorId} added successfully");
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while adding a new sponsor. Please try again later." + ex);
            }
        }

        [HttpPut("{SponsorId}")]
        public async Task<IActionResult> UpdateSponsor(SponsorResponseModel sponsorInputModel)
        {
            try
            {
                var success = await _sponsorRepository.UpdatedSponsor(sponsorInputModel);
                if (success)
                {
                    return Ok($"Sponsor with ID {sponsorInputModel.SponsorId} Updated successfully");
                }
                else
                {
                    return NotFound("Sponsor Not Found.");
                }
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while updating sponsor. Please try again later." + ex);
            }
        }

        [HttpDelete("{SponsorID}")]
        public async Task<IActionResult> DeleteSponsor(int SponsorID)
        {
            try
            {
                var success = await _sponsorRepository.DeleteSponsor(SponsorID);
                if (success)
                {
                    return Ok($"Sponsor with ID {SponsorID} has been deleted successfully");
                }
                else
                {
                    return NotFound("Sponsor Not Found.");
                }
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while deleting sponsor. Please try again later." + ex);
            }
        }

        [HttpGet("{SponsorID}")]
        public async Task<IActionResult> GetSponsorById(int SponsorID)
        {
            try
            {
                var sponsor = await _sponsorRepository.GetSponsorById(SponsorID);
                if (sponsor != null)
                {
                    return Ok(sponsor);
                }
                else
                {
                    return NotFound("Sponsor Not Found.");
                }
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while fetching sponsor. Please try again later." + ex);
            }
        }
    }
}
