using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMCEventsV1.Repository.Models;
using MMCEventsV1.Repository;
using System.Numerics;
using System.Reflection;
using MMCEventsV1.DTO.Speaker;
using MMCEventsV1.Repository.Repositories;
using MMCEventsV1.Repository.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MMCEventsV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeakerController : ControllerBase
    {

        private readonly ISpeakerRepository _speakerRepository;
        public SpeakerController(ISpeakerRepository speakerRepository)
        {
            _speakerRepository = speakerRepository;
        }
 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpeakerResponseModel>>> Get() //VERIFIED
        {
            try
            {
                var speakers = await _speakerRepository.GetSpeakers();
                if(speakers == null)
                {
                    return NotFound();
                }
                return Ok(speakers.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request." + ex.Message);
            }
        }

        // POST api/<SpeakerController>// verified double 
        [HttpPost]
        public async Task<IActionResult> AddSpeaker([FromBody] SpeakerInputModel inputModel)
        {
           var addedSpeaker = await _speakerRepository.AddSpeaker(inputModel);
            return Ok(addedSpeaker);
        }


    }
}
