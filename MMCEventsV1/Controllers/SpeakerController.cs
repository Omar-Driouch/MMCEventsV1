using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMCEventsV1.DTO;
using MMCEventsV1.Repository.Models;
using MMCEventsV1.Repository;
using System.Numerics;
using System.Reflection;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MMCEventsV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeakerController : ControllerBase
    {
        private readonly MMC_Event _Speaker;
        private readonly MMC_Event _SocialMedia;
        private readonly MMC_Event _User;

        public SpeakerController(MMC_Event context) { _Speaker = context; _SocialMedia = context; _User = context; }


        // GET ALL: api/<SpeakerController> // Verified 

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpeakerResponseModel>>> Get()
        {
            try
            {
                var speakers = await _Speaker.Speakers.ToListAsync();
                List<SpeakerResponseModel> speakerResponseModels = new List<SpeakerResponseModel>();

                foreach (var speaker in speakers)
                {
                    var socialMedia = await _SocialMedia.SocialMedia.FirstOrDefaultAsync(sm => sm.SpeakerId == speaker.SpeakerId);
                    var SpeakerInfoFromUser = await _User.Users.FirstOrDefaultAsync(sm => sm.UserId == speaker.SpeakerId);

                    SpeakerResponseModel responseModel = new SpeakerResponseModel
                    {


                        SpeakerID = speaker.SpeakerId,
                        FirstName = SpeakerInfoFromUser.FirstName,
                        LastName = SpeakerInfoFromUser.LastName,

                        SpeakerEmail = SpeakerInfoFromUser.UserEmail,
                        Phone = SpeakerInfoFromUser.Phone,
                         City = SpeakerInfoFromUser.City,
                        Gender = SpeakerInfoFromUser.Gender,

                        Picture = speaker.Picture,
                        Mct = (bool)speaker.Mct,
                        Mvp = (bool)speaker.Mvp,
                        Biography = speaker.Biography,
                        Instagram = socialMedia?.Instagram,
                        Facebook = socialMedia?.Facebook,
                        LinkedIn = socialMedia?.LinkedIn,
                        Website = socialMedia?.Website,
                        Twitter = socialMedia?.Twitter,


                    };

                    speakerResponseModels.Add(responseModel);
                }

                return Ok(speakerResponseModels);
            }
            catch (Exception ex)
            {
                // Handle exception appropriately, log or return an error response
                return StatusCode(500, "An error occurred while processing the request." + ex.Message);
            }
        }



        // POST api/<SpeakerController>// verified double 
        [HttpPost]
        public async Task<IActionResult> AddSpeaker([FromBody] SpeakerInputModel inputModel)
        {
            try
            {

                await _Speaker.Database.ExecuteSqlRawAsync("INSERT INTO Speakers (SpeakerID,Picture, MCT, MVP, BioGraphy) VALUES ({0}, {1}, {2}, {3}, {4})",
                    inputModel.SpeakerID, inputModel.Picture, inputModel.MCT, inputModel.MVP, inputModel.BioGraphy );

                await _Speaker.Database.ExecuteSqlRawAsync("INSERT INTO SocialMedia (SpeakerID, Facebook, Instagram, LinkedIn, Twitter, Website) VALUES ({0}, {1}, {2}, {3}, {4}, {5})",
                     inputModel.SpeakerID, inputModel.Facebook, inputModel.Instagram, inputModel.LinkedIn, inputModel.Twitter, inputModel.Website);

                return Ok(inputModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }



        // PUT api/<SpeakerController>// 
        [HttpPut("{SpeakerID}")]
        public async Task<IActionResult> UpdateSpeaker([FromBody] SpeakerInputModel inputModel)
        {
            try
            {
         
               
                var sp =  _Speaker.Speakers.Find(inputModel.SpeakerID);
                var sm = _SocialMedia.SocialMedia.Find(inputModel.SpeakerID);
                if(sm== null)
                {
                    await _Speaker.Database.ExecuteSqlRawAsync("INSERT INTO SocialMedia (SpeakerID, Facebook, Instagram, LinkedIn, Twitter, Website) VALUES ({0}, {1}, {2}, {3}, {4}, {5})",
                   inputModel.SpeakerID, inputModel.Facebook, inputModel.Instagram, inputModel.LinkedIn, inputModel.Twitter, inputModel.Website);
                 sm = _SocialMedia.SocialMedia.Find(inputModel.SpeakerID);
                }
                if (sp != null)
                {
                    sp.Mvp = inputModel.MVP;
                    sp.Mct = inputModel.MCT;
                    sp.Picture = inputModel.Picture;
                    sp.Biography = inputModel.BioGraphy;
                    sm.SpeakerId = (int)inputModel.SpeakerID;
                    sm.Instagram = inputModel.Instagram;
                    sm.Facebook = inputModel.Facebook;
                    sm.Twitter = inputModel.Twitter;
                    sm.LinkedIn = inputModel.LinkedIn;
                    sm.Website = inputModel.Website;

                     _Speaker.Speakers.Update(sp);
                    await _Speaker.SaveChangesAsync();
                     _SocialMedia.SocialMedia.Update(sm);
                    await _SocialMedia.SaveChangesAsync();
                    return Ok(inputModel);
                }
                else
                {
                    return NotFound("This Speaker not found"+inputModel.SpeakerID);
                }

               
              
              
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }




        // Get By ID api/<SpeakerController>/ verified
        [HttpGet("{SpeakerID}")]
        public async Task<ActionResult<SpeakerResponseModel>> GetSpeakerByID(int SpeakerID)
        {
            try
            {

                SpeakerResponseModel speakerResponseModel = new SpeakerResponseModel();
                var speakers = await _Speaker.Speakers.FindAsync(SpeakerID);

               if (speakers != null)
                {
                    var socialMedia = await _SocialMedia.SocialMedia.FindAsync(SpeakerID);
                    var SpeakerInfoFromUser = await _User.Users.FirstOrDefaultAsync(sm => sm.UserId == SpeakerID);

                    speakerResponseModel.SpeakerID = SpeakerID;

                    speakerResponseModel.FirstName = SpeakerInfoFromUser.FirstName;
                    speakerResponseModel.LastName = SpeakerInfoFromUser.LastName;
                    speakerResponseModel.SpeakerEmail = SpeakerInfoFromUser.UserEmail;
                    speakerResponseModel.Phone = SpeakerInfoFromUser.Phone;
                    speakerResponseModel.City = SpeakerInfoFromUser.City;
                    speakerResponseModel.Gender = SpeakerInfoFromUser.Gender;

                    speakerResponseModel.Picture = speakers?.Picture;
                    speakerResponseModel.Mct = (bool)speakers?.Mct;
                    speakerResponseModel.Mvp = (bool)speakers?.Mvp;
                    speakerResponseModel.Biography = speakers?.Biography;
                    speakerResponseModel.Instagram = socialMedia?.Instagram;
                    speakerResponseModel.Facebook = socialMedia?.Facebook;
                    speakerResponseModel.Website = socialMedia?.Website;
                    speakerResponseModel.LinkedIn = socialMedia?.LinkedIn;
                    speakerResponseModel.Twitter = socialMedia?.Twitter;

                    return Ok(speakerResponseModel);
                }
               else
                {
                    return NotFound("This Speaker Does not Exist ");
                }




            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


         
        // DELETE api/<SpeakerController>/ verified
        [HttpDelete("{SpeakerID}")]
        public async Task<ActionResult<Speaker>> DeleteSpeaker(int SpeakerID)
        {
            try
            {
                var speakerToDelete = await _Speaker.Speakers.FindAsync(SpeakerID);

                if (speakerToDelete == null)
                {
                    return NotFound("Speaker not found");
                }

                _Speaker.Speakers.Remove(speakerToDelete);
                await _Speaker.SaveChangesAsync();

                return Ok("Speaker deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
