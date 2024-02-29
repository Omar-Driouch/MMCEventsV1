using Microsoft.AspNetCore.Mvc;
using MMCEventsV1.DTO.Support;
using MMCEventsV1.Repository.Interfaces;
using MMCEventsV1.Repository.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMCEventsV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportController : ControllerBase
    {
        private readonly ISupportRepository _supportRepository;

        public SupportController(ISupportRepository supportRepository)
        {
            _supportRepository = supportRepository;
        }
        //VERIFIED
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupportResponseModel>>> GetSupport()
        {
            try
            {
                var supports = await _supportRepository.GetAll();
                if (supports != null)
                {
                    return Ok(supports.Value);
                }
                else
                {
                    return NotFound("No supports found.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or return a proper error response
                throw new Exception("An error occurred while fetching supports: " + ex.Message);
            }
        }



        //VERIFIED
        [HttpPost]
        public async Task<IActionResult> AddNewSupport(SupportInputModel supportInputModel)
        {
            try
            {
                var supportId = await _supportRepository.AddNewSupport(supportInputModel);
                if (supportId)
                    return Ok($"Support {supportInputModel.Name} added successfully");
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "An error occurred while adding a new support. Please try again later." + ex);
            }
        }



        //VERIFIED  
        [HttpPut("{SupportId}")]
        public async Task<IActionResult> UpdateSupport(SupportResponseModel supportInputModel)
        {
            try
            {
                var supportId = await _supportRepository.UpdateSupport(supportInputModel);
                if (supportId)
                {
                    return Ok($"Support with ID {supportInputModel.SupportId} updated successfully");
                }
                else
                {
                    return NotFound("Support not found.");
                }
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while updating support. Please try again later." + ex);
            }
        }

        [HttpDelete("{SupportID}")]
        public async Task<IActionResult> DeleteSupport(int SupportID)
        {
            try
            {
                var supportId = await _supportRepository.DeleteSupport(SupportID);
                if (supportId)
                {
                    return Ok($"Support with ID {SupportID} has been deleted successfully");
                }
                else
                {
                    return NotFound("Support not found.");
                }
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while deleting support. Please try again later." + ex);
            }
        }

        [HttpGet("{SupportID}")]
        public async Task<IActionResult> GetSupportById(int SupportID)
        {
            try
            {
                var support = await _supportRepository.GetSupportById(SupportID);
                if (support != null)
                {
                    return Ok(support);
                }
                else
                {
                    return NotFound("Support not found.");
                }
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response
                return StatusCode(500, "An error occurred while fetching support. Please try again later." + ex);
            }
        }
    }
}
