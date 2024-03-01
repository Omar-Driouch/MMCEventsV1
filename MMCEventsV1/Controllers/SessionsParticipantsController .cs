using Microsoft.AspNetCore.Mvc;
using MMCEventsV1.DTO.SessionsParticipants;
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

        [HttpGet("SpeakersParticipants")]
        public async Task<ActionResult<IEnumerable<SessionsParticipantsResponseModel>>> GetAllSessionsParticipantsBySpeaker()
        {
            try
            {
                var sessionParticipants = await _sessionsParticipantsRepository.GetAllSessionsParticipantsBySpeaker();
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

        [HttpPost]
        public async Task<IActionResult> AddNewSessionsParticipants(SessionsParticipantsInputModel inputModel)
        {
            try
            {
                int id = await _sessionsParticipantsRepository.AddNewSessionsParticipants(inputModel);
                return Ok($"Session participant with ID {id} added successfully.");
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSessionsParticipantsByID(int id)
        {
            try
            {
                var success = await _sessionsParticipantsRepository.DeleteSessionsParticipantsByID(id);
                if (success)
                {
                    return Ok($"Session participant with ID {id} deleted successfully.");
                }
                else
                {
                    return NotFound($"Session participant with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting session participant by ID. " + ex.Message);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetSessionsParticipantsByUser(int userId)
        {
            try
            {
                var sessionParticipant = await _sessionsParticipantsRepository.GetSessionsParticipantsByUser(userId);
                if (sessionParticipant != null)
                {
                    return Ok(sessionParticipant);
                }
                else
                {
                    return NotFound($"Session participant for user with ID {userId} not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching session participant by user. " + ex.Message);
            }
        }

        // Add other actions as per your requirements
    }
}
