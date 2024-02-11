using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MMCEventsV1.DTO;
using MMCEventsV1.Repository;
using MMCEventsV1.Repository.Models;

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
            _Session= context;
        }
        // GET: api/<SessionController> // Verified 
      [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionResponseModel>>> Get()
        {
            var sessions =  _Session.Sessions.ToList();
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
