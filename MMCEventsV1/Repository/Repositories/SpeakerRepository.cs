using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MMCEventsV1.DTO.Speaker;
using MMCEventsV1.Repository.Interfaces;
using MMCEventsV1.Repository.Models;
using ScaffoldConcept.TestModels;

namespace MMCEventsV1.Repository.Repositories
{
    public class SpeakerRepository : ISpeakerRepository
    {
        private readonly MMC_Event _Speaker;
        
        public SpeakerRepository (MMC_Event speaker)
        {
            _Speaker = speaker;
        }
        public async Task<bool> AddSpeaker([FromBody] SpeakerInputModel inputModel)
        {
            try
            {
                await _Speaker.Database.ExecuteSqlRawAsync("INSERT INTO Speakers (SpeakerID, Picture, MCT, MVP, BioGraphy) VALUES ({0}, {1}, {2}, {3}, {4})",
                    inputModel.SpeakerID, inputModel.Picture, inputModel.MCT, inputModel.MVP, inputModel.BioGraphy);

                await _Speaker.Database.ExecuteSqlRawAsync("INSERT INTO SocialMedia (SpeakerID, Facebook, Instagram, LinkedIn, Twitter, Website) VALUES ({0}, {1}, {2}, {3}, {4}, {5})",
                    inputModel.SpeakerID, inputModel.Facebook, inputModel.Instagram, inputModel.LinkedIn, inputModel.Twitter, inputModel.Website);

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return false;
            }
        }



        public Task<ActionResult<Speaker>> DeleteSpeaker(int SpeakerID)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<SpeakerResponseModel>> GetSpeakerByID(int SpeakerID)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult<ICollection<SpeakerResponseModel>>> GetSpeakers() // VERIFIED
        {
            try
            {
                var speakers = await _Speaker.Speakers.ToListAsync();

                if (speakers == null || !speakers.Any())
                {
                    return null;
                }

                List<SpeakerResponseModel> speakerResponseModels = new();

                foreach (var speaker in speakers)
                {
                    var socialMedia = await _Speaker.SocialMedia.FirstOrDefaultAsync(sm => sm.SpeakerId == speaker.SpeakerId);
                    var speakerInfoFromUser = await _Speaker.Users.FirstOrDefaultAsync(u => u.UserId == speaker.SpeakerId);

                    SpeakerResponseModel responseModel = new SpeakerResponseModel
                    {
                        SpeakerID = speaker.SpeakerId,
                        FirstName = speakerInfoFromUser?.FirstName,
                        LastName = speakerInfoFromUser?.LastName,
                        SpeakerEmail = speakerInfoFromUser?.UserEmail,
                        Phone = speakerInfoFromUser?.Phone,
                        City = speakerInfoFromUser?.City,
                        Gender = speakerInfoFromUser?.Gender,
                        Picture = speaker.Picture,
                        Mct = (bool)speaker?.Mct,
                        Mvp = (bool)speaker?.Mvp,
                        Biography = speaker.Biography,
                        Instagram = socialMedia?.Instagram,
                        Facebook = socialMedia?.Facebook,
                        LinkedIn = socialMedia?.LinkedIn,
                        Website = socialMedia?.Website,
                        Twitter = socialMedia?.Twitter,
                    };

                    speakerResponseModels.Add(responseModel);
                }

                return speakerResponseModels;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching speakers.", ex);
            }
        }


        public Task<IActionResult> UpdateSpeaker([FromBody] SpeakerInputModel inputModel)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAsync() //VERIFIED
        {
            try
            {
                var saved = await _Speaker.SaveChangesAsync();
                return saved > 0;
            }
            catch (DbUpdateException)
            {

                return false;
            }
        }
    }
}
