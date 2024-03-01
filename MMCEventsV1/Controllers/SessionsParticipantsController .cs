using Microsoft.AspNetCore.Mvc;
using MMCEventsV1.DTO.SessionsParticipants;
using MMCEventsV1.DTO.User;
using MMCEventsV1.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMCEventsV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsParticipantsController : ControllerBase
    {
        private readonly ISessionsParticipants _sessionsParticipantsRepository;

        public SessionsParticipantsController(ISessionsParticipants sessionsParticipantsRepository)
        {
            _sessionsParticipantsRepository = sessionsParticipantsRepository;
        }

        // VERIFIED
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionsParticipantsResponseModel>>> GetAllSessionsParticipants()
        {
            try
            {
                var sessionParticipants = await _sessionsParticipantsRepository.GetAllSessionsParticipants();
                if (sessionParticipants != null)
                {
                    return Ok(sessionParticipants.Value);
                }
                else
                {
                    return NotFound("No session participants found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching session participants. " + ex.Message);
            }
        }
        // VERIFIED
        [HttpGet("Speakers/{SessionID}")]
        public async Task<ActionResult<IEnumerable<SessionsParticipantsResponseModel>>> GetAllSessionsParticipantsBySpeaker(int SessionID)
        {
            try
            {
                var sessionParticipants = await _sessionsParticipantsRepository.GetAllSessionsParticipantsBySpeaker(SessionID);
                if (sessionParticipants?.Value?.Count() > 0)
                {
                    return Ok(sessionParticipants.Value);
                }
                else
                {
                    return NotFound("No session speaker found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching session participants. " + ex.Message);
            }
        }



        [HttpPost]
        public async Task<IActionResult> AddNewSessionsParticipants(SessionsParticipantsInputModel inputModel)
        {
            try
            {
                var  isAdded = await _sessionsParticipantsRepository.AddNewSessionsParticipants(inputModel);
                if (isAdded)
                {
                return Ok($"User with ID {inputModel.UserID} has been added to session {inputModel.SessionID}");

                }else
                {
                    return BadRequest($"User is Already participated to this Session");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding a new session participant. " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSessionsParticipants(int id, SessionsParticipantsResponseModel inputModel)
        {
            try
            {
                inputModel.SessionsParticipantsID = id;
                var success = await _sessionsParticipantsRepository.UpdatedSessionsParticipants(inputModel);
                if (success)
                {
                    return Ok($"Session participant with ID {id} updated successfully.");
                }
                else
                {
                    return NotFound($"Session participant with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating session participant. " + ex.Message);
            }
        }
        //DONE
        [HttpDelete("Users/{SessionID}/{UserID}")]
        public async Task<IActionResult> DeleteUserFromSessionByUserID(int SessionID, int UserID)
        {
            try
            {
                var success = await _sessionsParticipantsRepository.DeleteUserFromSessionByUserID(SessionID, UserID);
                if (success)
                {
                    return Ok($"User participant with ID {UserID} deleted successfully from session SessionID.");
                }
                else
                {
                    return NotFound($"User participant with ID {UserID} not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting session participant by ID. " + ex.Message);
            }
        }



        //DONE
        [HttpDelete("Users/{SessionID}")]
        public async Task<IActionResult> DeleteAllUserFromSessionByUserID(int SessionID)
        {
            try
            {
                var success = await _sessionsParticipantsRepository.DeleteAllUserFromSessionByUserID(SessionID);
                if (success)
                {
                    return Ok($"Session with ID {SessionID} deleted successfully from session Particpipated.");
                }
                else
                {
                    return NotFound($"Session  with ID {SessionID} not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting session participant by ID. " + ex.Message);
            }
        }

 

        //DONE
        [HttpGet("Users/{SessionID}")]
        public async Task<ActionResult<IEnumerable<UserResponseModel>>?> GetAllSessionsParticipantsByUsers(int SessionID)
        {
            try
            {
                var sessionParticipants = await _sessionsParticipantsRepository.GetAllSessionsParticipantsByUser(SessionID);
                if (sessionParticipants?.Value != null)
                {
                    return Ok(sessionParticipants.Value);
                }
                else
                {
                    return NotFound("No users found in this Session.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching session participants. " + ex.Message);
            }
        }



    }
}
