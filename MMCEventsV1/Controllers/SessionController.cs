﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MMCEventsV1.DTO.Session;
using MMCEventsV1.Repository;
using MMCEventsV1.Repository.Interfaces;
using MMCEventsV1.Repository.Models;
using System.ComponentModel.DataAnnotations;



namespace MMCEventsV1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        public readonly ISessionsRepository _sessionsRepository;
        public SessionController(ISessionsRepository sessionsRepository)
        {
            _sessionsRepository = sessionsRepository;
        }

        //GET VERIFIED 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionResponseModel>>> Get()
        {
            try
            {
                var sessions = await _sessionsRepository.GetSessions();
                if (sessions == null) { return NotFound(); }
                return Ok(sessions.Value);
            }
            catch (Exception ex)
            {

                throw new Exception("Internal server error: " + ex.Message);
            }

        }

        //POST VERIFIED
        [HttpPost("{EventID}")]
        public async Task<ActionResult<string>> AddNewSession([FromBody] SessionInputModel sessionInputModel)
        {
            try
            {
                var isAdded = await _sessionsRepository.AddNewSession(sessionInputModel);
                if (isAdded)
                {
                    return Ok("The session has been added successfully");
                }
                else
                {
                    return BadRequest("Something went wrong while adding the session");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding a new session: " + ex.Message);
            }
        }


        //GET SESSION BY EVENT VERIFIED
        [HttpGet("{EventID}")]
        public async Task<ActionResult<IEnumerable<SessionResponseModel>>> GetSessionByEvent(int EventID)
        {
            try
            {
                var AllSessionbyEvent = await _sessionsRepository.GetSessionByEvent(EventID);
                if (AllSessionbyEvent != null)
                {
                    return Ok(AllSessionbyEvent.Value);
                }
                else
                {
                    return NotFound("Event maybe not exist");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching sessions by event: " + ex.Message);
            }
        }

        //DELETE VERIFIED
        [HttpDelete("{SessionID}")]
        public async Task<IActionResult> DeleteSession(int SessionID)
        {
            try
            {
                var isDeleted =  await _sessionsRepository.DeleteSession(SessionID);
                if (isDeleted)
                {
                    return Ok("Session Deleted successfuly");

                }
                else { return BadRequest() ; }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //UPDATE VERIFIED
        [HttpPut("{SessionID}")]
        public async Task<IActionResult> UpdateSession([FromBody] SessionsUpdateModel sessionsUpdateModel)
        {
            try
            {
                var isUpdated = await _sessionsRepository.UpdateSession(sessionsUpdateModel);
                if (isUpdated)
                {
                    return Ok("updated successfully");
                }
                else
                {
                    return BadRequest("Failed to update session");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in the Updated Method failed to update session "+ex);
            }


        }



    }


}
