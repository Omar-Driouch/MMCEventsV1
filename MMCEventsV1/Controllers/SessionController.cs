using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MMCEventsV1.DTO;
using MMCEventsV1.Repository;
using MMCEventsV1.Repository.Models;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MMCEventsV1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        public readonly MMC_Event _Session;
        public SessionController(MMC_Event context)
        {
            _Session = context;
        }
        // GET: api/<SessionController> // Verified 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionResponseModel>>> Get()
        {
            var sessions = _Session.Sessions.ToList();
            if (sessions == null || !sessions.Any())
            {
                return NotFound();
            }

            var responseModels = sessions.Select(session => new SessionResponseModel
            {
                SessionID = session.SessionId,
                Title = session.Title,
                DateSession = session.DateSession,
                Description = session.Description,
                Address = session.Address,
                Picture = session.Picture,

            }).ToList();

            return Ok(responseModels);
        }


        //psot Sessions Verified 
        [HttpPost("{EventID}")]
        public async Task<ActionResult<string>> AddNewSession([FromBody] SessionInputModel sessionInputModel, int EventID)
        {
            try
            {
                if (sessionInputModel == null)
                {
                    return BadRequest("Data must not be null");
                }
                var findEvent = await _Session.Events.FindAsync(EventID);
                if (findEvent==null)
                {
                    return NotFound("Event Does not exist");
                }
                Session NewSession = new Session
                {
                    Address = sessionInputModel.Address,
                    Title = sessionInputModel.Title,
                    DateSession = sessionInputModel.DateSession,
                    Description = sessionInputModel.Description,
                    Picture = sessionInputModel.Picture,
                    EventId = EventID
                };

                await _Session.Sessions.AddAsync(NewSession);
                await _Session.SaveChangesAsync();

                return Ok("The session has been added successfully");
            }
            catch (Exception err)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding a new session." + err.Message);
            }
        }


        //Get  Sessions by ID   // VERIFIED 
        [HttpGet("{SessionID}")] 
        public async Task<ActionResult<SessionResponseModel>> GetSessionByID(int SessionID)
        {
            try
            {
                var findSessionById= await _Session.Sessions.FindAsync(SessionID);

                if (findSessionById == null)
                {
                    return NotFound("Session Not Found ");
                }

                SessionResponseModel FoundSession = new SessionResponseModel
                {
                    SessionID= findSessionById.SessionId,
                    Address = findSessionById.Address,
                    Title = findSessionById.Title,
                    DateSession = findSessionById.DateSession,
                    Description = findSessionById.Description,
                    Picture = findSessionById.Picture,

                };

                 

                return Ok(FoundSession);
            }
            catch (Exception err)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding a new session." + err.Message);
            }
        }


        // HttpPut: api/<SessionController> // Verified
        [HttpPut("{SessionID}")]
        public async Task<ActionResult<string>> UpdateSession([FromBody] SessionInputModel sessionInputModel , int SessionID)
        {
            var SessionToUpdate = await _Session.Sessions.FindAsync(SessionID);
            try
            {
                if (SessionToUpdate == null)
                {
                    return NotFound();
                }

                SessionToUpdate.Picture = sessionInputModel.Picture;
                SessionToUpdate.Title = sessionInputModel.Title;
                SessionToUpdate.Address = sessionInputModel.Address;
                SessionToUpdate.DateSession = sessionInputModel.DateSession;
                SessionToUpdate.Description = sessionInputModel.Description;

                await _Session.SaveChangesAsync();
                return Ok("the session has been updated successfully !");
            }
            catch (Exception err)
            {

                return BadRequest(err.Message);
            }
        }

        // HttpDelete: api/<SessionController> // Verified 
        [HttpDelete("{SessionID}")]
        public async Task<ActionResult<string>> DeleteSession(int SessionID)
        {
            var Deleted = _Session.Sessions.Find(SessionID);
            if (Deleted == null)
            {
                return NotFound();
            }
            try
            {
                // find and delete all the spnsorSesison and support session  related to that table 
                var SupportSessionsToDelete = await _Session.SupportSessions.Where(es => es.SessionId == SessionID).ToListAsync();
                if (SupportSessionsToDelete.Any())
                {
                    _Session.SupportSessions.RemoveRange(SupportSessionsToDelete);
                    await _Session.SaveChangesAsync();
                }

                var SponsorSessionsToDelete = await _Session.SponsorSessions.Where(es => es.SessionId == SessionID).ToListAsync();
                if (SponsorSessionsToDelete.Any())
                {
                    _Session.SponsorSessions.RemoveRange(SponsorSessionsToDelete);
                    await _Session.SaveChangesAsync();
                }

                _Session.Sessions.Remove(Deleted);
                await _Session.SaveChangesAsync();

                return Ok("Session Deleted Successfully");
            }
            catch (Exception err)
            {

                return BadRequest(err.Message);
            }
        }

    }
}
