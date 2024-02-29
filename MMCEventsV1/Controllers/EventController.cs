using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMCEventsV1.DTO.Events;
using MMCEventsV1.Repository;
using MMCEventsV1.Repository.Interfaces;
using MMCEventsV1.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMCEventsV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventsRepository _eventsRepository;
        public EventController(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }

        // VERIFIED
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventInputModel>>> GetEvents()
        {
            try
            {
               var events = await _eventsRepository.GetEvents();
                if (events != null) { return Ok(events.Value); } else { return BadRequest("Errors"); }
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // VERIFIED
        [HttpGet("{EventID}")]
        public async Task<IActionResult> GetEventById(int EventID)
        {
            try
            {
                var ev =  await _eventsRepository.GetEventById(EventID);
                if (ev == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(ev.Value);
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erorr from the server ", ex);
            }
 

            
        }


        // VERIFIED
        [HttpPost]
        public async Task<ActionResult<EventUpdateModel>> CreateEvent(EventUpdateModel eventUpdateModel)
        {
            try
            {
                if (eventUpdateModel == null)
                {
                    return BadRequest("Event data is missing");
                }
                else
                {
                    var isAdded = await _eventsRepository.CreateEvent(eventUpdateModel);
                    if (isAdded)
                    {
                        return Ok(eventUpdateModel);
                    }
                    else
                    {
                        return StatusCode(500, "Internal server error: Failed to add the event.");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // VERIFIED
        [HttpPut("{EventID}")]
        public async Task<IActionResult> UpdateEvent( EventInputModel inputModel)
        {
            try
            {
                var isUpdated = await _eventsRepository.UpdateEvent( inputModel);
                if (isUpdated)
                {
                    return Ok("The Event has been updated");
                }
                else
                {
                    return NotFound("The Event does not exist");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Internal server error: " + ex.Message);
            }
        }


        // VERIFIED
        [HttpDelete("{EventID}")]
        public async Task<IActionResult> DeleteEvent(int EventID)
        {
            try
            {
                var isDeleted = await _eventsRepository.DeleteEvent(EventID);
                if (isDeleted)
                {
                    return Ok("The Event has been deleted successfully");
                }
                else
                {
                    return NotFound("The Event does not exist");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}
