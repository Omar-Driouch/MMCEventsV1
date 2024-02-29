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
        public async Task<bool> AddSpeaker([FromBody] SpeakerInputModel inputModel) // VERIFEID
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

        public async Task<bool> DeleteSpeaker(int SpeakerID) //VERIFEID
        {
            try
            {
                var speakerToDelete = await _Speaker.Speakers.FindAsync(SpeakerID);
                var SocdialeMedia = await _Speaker.SocialMedia.FindAsync(SpeakerID);
                if (speakerToDelete == null)
                {
                    return false;
                }
                if (SocdialeMedia != null)
                {
                    _Speaker.SocialMedia.Remove(SocdialeMedia);
                }
                _Speaker.Speakers.Remove(speakerToDelete);
               var saved =  await _Speaker.SaveChangesAsync();

                return saved > 0;
            }
            catch (Exception)
            {

                throw new Exception("Error while deleting speaker");
            }
        }

        public async Task<SpeakerResponseModel> GetSpeakerByID(int SpeakerID) //VERIFEID
        {
            try
            {
                var findSpeaker = await _Speaker.Speakers.FindAsync(SpeakerID);
                if (findSpeaker != null)
                {
                    SpeakerResponseModel speakerResponseModel = new ();
                    var socialMedia = await _Speaker.SocialMedia.FindAsync(SpeakerID);
                    var SpeakerInfoFromUser = await _Speaker.Users.FirstOrDefaultAsync(sm => sm.UserId == SpeakerID);
                    speakerResponseModel.SpeakerID = SpeakerID;
                    //speaker information from the table users 
                    speakerResponseModel.FirstName = SpeakerInfoFromUser.FirstName;
                    speakerResponseModel.LastName = SpeakerInfoFromUser.LastName;
                    speakerResponseModel.SpeakerEmail = SpeakerInfoFromUser.UserEmail;
                    speakerResponseModel.Phone = SpeakerInfoFromUser.Phone;
                    speakerResponseModel.City = SpeakerInfoFromUser.City;
                    speakerResponseModel.Gender = SpeakerInfoFromUser.Gender;
                    //speaker info from table speakers
                    speakerResponseModel.Picture = findSpeaker?.Picture;
                    speakerResponseModel.Mct = (bool)findSpeaker?.Mct;
                    speakerResponseModel.Mvp = (bool)findSpeaker?.Mvp;
                    speakerResponseModel.Biography = findSpeaker?.Biography;
                    //speaker info from social media table 
                    speakerResponseModel.Instagram = socialMedia?.Instagram;
                    speakerResponseModel.Facebook = socialMedia?.Facebook;
                    speakerResponseModel.Website = socialMedia?.Website;
                    speakerResponseModel.LinkedIn = socialMedia?.LinkedIn;
                    speakerResponseModel.Twitter = socialMedia?.Twitter;

                    return speakerResponseModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
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

        public async Task<bool> UpdateSpeaker(SpeakerInputModel inputModel) //VERIFIED
        {
            try
            {
                var speakerToUpdate =  _Speaker.Speakers.Find(inputModel.SpeakerID);
                if (speakerToUpdate != null)
                {
                    _Speaker.Entry(speakerToUpdate).State = EntityState.Detached;
                    var socialMedia = new SocialMedia
                    {
                        SpeakerId = inputModel.SpeakerID,
                        Facebook = inputModel.Facebook,
                        Instagram = inputModel.Instagram,
                        Twitter = inputModel.Twitter,
                        Website = inputModel.Website,
                        LinkedIn = inputModel.LinkedIn
                    };

                    var updatedSpeaker = new Speaker
                    {
                        SpeakerId = inputModel.SpeakerID,
                        Picture = inputModel.Picture,
                        Mct = inputModel.MCT,
                        Mvp = inputModel.MVP,
                        Biography = inputModel.BioGraphy,
                        SocialMedia = socialMedia
                    };

                    _ =  _Speaker.Speakers.Update(updatedSpeaker);
                    var saved = await _Speaker.SaveChangesAsync();

                    return saved > 0;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw new Exception("Error while updating speaker");
            }
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
