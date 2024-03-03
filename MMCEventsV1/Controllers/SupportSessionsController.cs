using Microsoft.AspNetCore.Mvc;
using MMCEventsV1.DTO.SessionsParticipants;
using MMCEventsV1.DTO.SupportSession;
using MMCEventsV1.DTO.User;
using MMCEventsV1.Repository.Interfaces;
using MMCEventsV1.Repository.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMCEventsV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportSessionsController : ControllerBase
    {
        private readonly ISessionsSupportRepo _sessionsSupportRepository;

        public SupportSessionsController(ISessionsSupportRepo sessionsSupportRepository)
        {
            _sessionsSupportRepository = sessionsSupportRepository;
        }

        // VERIFIED
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionsParticipantsResponseModel>>> GetAllSessionsParticipants()
        {
            try
            {
                var sessionSupport = await _sessionsSupportRepository.GetAllSessionsSupports();
                if (sessionSupport != null)
                {
                    return Ok(sessionSupport.Value);
                }
                else
                {
                    return NotFound("No session Support found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching session Support . " + ex.Message);
            }
        }





        //[HttpGet("Speakers/{SessionID}")]
        //public async Task<ActionResult<IEnumerable<SessionsParticipantsResponseModel>>> GetAllSessionsParticipantsBySpeaker(int SessionID)
        //{
        //    try
        //    {
        //        var sessionParticipants = await _sessionsParticipantsRepository.GetAllSessionsParticipantsBySpeaker(SessionID);
        //        if (sessionParticipants?.Value?.Count() > 0)
        //        {
        //            return Ok(sessionParticipants.Value);
        //        }
        //        else
        //        {
        //            return NotFound("No session speaker found.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "An error occurred while fetching session participants. " + ex.Message);
        //    }
        //}


        //DONE
        [HttpPost]
        public async Task<IActionResult> AddNewSupportSession(SupportSessionInputModel inputModel)
        {
            try
            {
                var isAdded = await _sessionsSupportRepository.AddNewSessionsSupport(inputModel);
                if (isAdded)
                {
                    return Ok($"Support with ID {inputModel.SupportID} has been added to session {inputModel.SessionID}");

                }
                else
                {
                    return BadRequest($"Support is Already Added to this Session");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding a new session Support. " + ex.Message);
            }
        }

        //DONE
        [HttpDelete("SessionID/{SessionID}")]
        public async Task<IActionResult> DeleteSessionSupportsBySessionID(int SessionID)
        {
            try
            {
                var success = await _sessionsSupportRepository.DeleteSupportSessionBySessionID(SessionID);
                if (success)
                {
                    return Ok($"Session Supports with SessionID  {SessionID} deleted successfully .");
                }
                else
                {
                    return NotFound($"SessionID  with ID {SessionID} not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting session Support by ID. " + ex.Message);
            }
        }

        //DONE
        [HttpDelete("SupportID/{SupportID}")]
        public async Task<IActionResult> DeleteSessionSupportsBySupportID(int SupportID)
        {
            try
            {
                var success = await _sessionsSupportRepository.DeleteSessionSupportsBySupportID(SupportID);
                if (success)
                {
                    return Ok($"Session Supports with SupportID  {SupportID} deleted successfully .");
                }
                else
                {
                    return NotFound($"SupportID  with ID {SupportID} not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting session Support by ID. " + ex.Message);
            }
        }



        //DONE
        [HttpGet("SupportID/{SupportID}")]
        public async Task<ActionResult<IEnumerable<SupportSessionResponseModel>>?> GetAllSessionSupportsBySupportID(int SupportID)
        {
            try
            {
                var sessionParticipants = await _sessionsSupportRepository.GetAllSessionSupportsBySupportID(SupportID);
                if (sessionParticipants?.Value != null)
                {
                    return Ok(sessionParticipants.Value);
                }
                else
                {
                    return NotFound("No Support Session found in this With this SupportID .");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching session Support Session . " + ex.Message);
            }
        }


        //DONE
        [HttpGet("SessionID/{SessionID}")]
        public async Task<ActionResult<IEnumerable<SupportSessionResponseModel>>?> GetAllSessionSupportsBySessionID(int SessionID)
        {
            try
            {
                var sessionParticipants = await _sessionsSupportRepository.GetAllSessionSupportsBySessionID(SessionID);
                if (sessionParticipants?.Value != null)
                {
                    return Ok(sessionParticipants.Value);
                }
                else
                {
                    return NotFound("No Support Session found in this With this SessionID .");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching session Support Session . " + ex.Message);
            }
        }


    }
}
