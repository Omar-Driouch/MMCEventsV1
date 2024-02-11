using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMCEventsV1.DTO;
using MMCEventsV1.Repository;
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
        private readonly MMC_Event _eventContext;
        private readonly MMC_Event _eventPartnerContext;
        private readonly MMC_Event _sessionContext;
        private readonly MMC_Event _supprtSession;
        private readonly MMC_Event _sponsorSessionContext;



        public EventController(MMC_Event context)
        {
            _eventContext = context;
            _eventPartnerContext = context;
            _sessionContext = context;
            _supprtSession = context;
            _sponsorSessionContext = context;
        }

        // GET: api/Event // Verified 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventInputModel>>> GetEvents()
        {
            try
            {
                var events = _eventContext.Events.ToList();
                if (events == null)
                {
                    return NotFound();
                }

                List<EventInputModel> eventResponseList = new List<EventInputModel>();

                foreach (var ev in events)
                {
                    EventInputModel eventResponseModel = new EventInputModel
                    {
                        EventID = ev.EventId,
                        Title = ev?.Title,
                        Description = ev?.Description,
                        Picture = ev?.Picture,
                        StartDate = (DateTime)ev?.StartDate,
                        EndDate = (DateTime)ev?.EndDate
                    };

                    eventResponseList.Add(eventResponseModel);
                }

                return Ok(eventResponseList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Event/{eventId}
        [HttpGet("{EventID}")]
        public async Task<ActionResult<EventInputModel>> GetEventById(int EventID)
        {
            var ev = _eventContext.Events.Find(EventID);
            if (ev == null)
            {
                return NotFound();
            }

            EventInputModel eventResponseModel = new EventInputModel
            {
                EventID = ev.EventId,
                Title = ev.Title,
                Description = ev?.Description,
                Picture = ev?.Description,
                StartDate = (DateTime)ev?.StartDate,
                EndDate = (DateTime)ev?.EndDate


            };

            return Ok(eventResponseModel);
        }

        // POST: api/Event // Verified 
        [HttpPost]
        public async Task<ActionResult<EventUpdateModel>> CreateEvent(EventUpdateModel eventUpdateModel)
        {
            try
            {
                if (eventUpdateModel == null)
                {
                    return BadRequest("Event data is missing");
                }

                // Create a new Event entity and map properties from EventUpdateModel
                var ev = new Event
                {
                    Title = eventUpdateModel?.Title,
                    Description = eventUpdateModel?.Description,
                    Picture = eventUpdateModel?.Picture,
                    StartDate = eventUpdateModel?.StartDate,
                    EndDate = eventUpdateModel?.EndDate
                };

                // Add the new event to the context and save changes
                _eventContext.Events.Add(ev);
                await _eventContext.SaveChangesAsync();

                return Ok("The Event has been added.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        // PUT: api/Event/{eventId} // Verified 
        [HttpPut("{eventId}")]
        public async Task<IActionResult> UpdateEvent(int eventId, EventInputModel inputModel)
        {
            try
            {
                var ev = _eventContext.Events.Find(eventId);
                if (ev == null)
                {
                    return NotFound("Event not found");
                }

                ev.Title = inputModel?.Title;
                ev.Description = inputModel?.Description;
                ev.Picture = inputModel?.Picture;
                ev.StartDate = inputModel?.StartDate;
                ev.EndDate = inputModel?.EndDate;
                // Update other properties

                _eventContext.Events.Update(ev);
                await _eventContext.SaveChangesAsync();



                return Ok(inputModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        // DELETE: api/Event/{eventId} Verified and Tested 
        [HttpDelete("{eventId}")]
        public async Task<ActionResult<string>> DeleteEvent(int eventId)
        {
            try {   

                   var evToDelete =  _eventContext.Events.Find(eventId);
                    if (evToDelete != null)
                    {

                            {
                                //--- find and Delete all the event from EventPartner Table ---
                                var evtPartnersToDelete = await _eventPartnerContext.EventPartners
                                                    .Where(ep => ep.EventId == eventId)
                                                    .ToListAsync();
                                foreach (var item in evtPartnersToDelete)
                                {
                                    _eventPartnerContext.EventPartners.Remove(item);
                                    await _eventPartnerContext.SaveChangesAsync();
                                 }

                                //-------------------------------------------------------
                             }

                    {
                        //--- find and Delete all the SupportSession from SupportSession Table  ---
                        var SessionToDelete = await _sessionContext.Sessions
                                            .Where(ep => ep.EventId == eventId)
                                            .ToListAsync();
                        foreach (var item in SessionToDelete)
                        {
                            var SupportSessionToDelete = await _supprtSession.SupportSessions
                                           .Where(ep => ep.SessionId == item.SessionId)
                                           .ToListAsync();
                            foreach (var sprt in SupportSessionToDelete)
                            {
                                _supprtSession.SupportSessions.Remove(sprt);
                                await _supprtSession.SaveChangesAsync();
                            }
                            //--- find and Delete all the SposnorSession from SposnorSession Table  ---
                            var SposnorSessionList = await _sponsorSessionContext.SponsorSessions
                                                .Where(sps => sps.SessionId == item.SessionId)
                                                .ToListAsync();
                            foreach (var SponsorSessionToDelete in SposnorSessionList)
                            {
                                    _sponsorSessionContext.SponsorSessions.Remove(SponsorSessionToDelete);
                                    await _sponsorSessionContext.SaveChangesAsync();
                            }
                        }
                        //-------------------------------------------------------
                    }

                   
                    _eventContext.Events.Remove(evToDelete);
                        await _eventContext.SaveChangesAsync();
                        return Ok("Event deleted successfully");
                    }
                    else
                    {
                        return NotFound("Event not found");
                    }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
