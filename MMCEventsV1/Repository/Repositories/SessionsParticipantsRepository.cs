using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMCEventsV1.DTO.Session;
using MMCEventsV1.DTO.SessionsParticipants;
using MMCEventsV1.DTO.Speaker;
using MMCEventsV1.DTO.User;
using MMCEventsV1.Repository.Interfaces;
using MMCEventsV1.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMCEventsV1.Repository.Repositories
{
    public class SessionsParticipantsRepository : ISessionsParticipants
    {
        private readonly MMC_Event _context;

        public SessionsParticipantsRepository(MMC_Event context)
        {
            _context = context;
        }
        //DONE
        public async Task<ActionResult<IEnumerable<SessionsParticipantsResponseModel>>?> GetAllSessionsParticipants()
        {
            try
            {
                var sessionsParticipants = await _context.SessionsParticipants.ToListAsync();

                if (sessionsParticipants.Any())
                {
                    var result = sessionsParticipants.Select(item => new SessionsParticipantsResponseModel
                    {
                        SessionsParticipantsID = item.ParticipateId,
                        SessionID = item.SessionId ?? 0,
                        UserID = item.UserId ?? 0
                    }).ToList();

                    return result;
                }
                else
                {
                    return new List<SessionsParticipantsResponseModel>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while fetching session participants: " + ex.Message);
            }
        }
        //DONE
        public async Task<bool> AddNewSessionsParticipants(SessionsParticipantsInputModel InputModel)
        {
            try
            {
                var sessionParticipant = new SessionsParticipant
                {
                    SessionId = InputModel.SessionID,
                    UserId = InputModel.UserID
                };

                await _context.SessionsParticipants.AddAsync(sessionParticipant);
               var saved =  await _context.SaveChangesAsync();

                return saved > 0;
            }
            catch (Exception ex)
            {
               
                throw new Exception("Error occurred while adding a new session participant: " + ex.Message);
            }
        }
        //DONE
        public async Task<bool> UpdatedSessionsParticipants(SessionsParticipantsResponseModel InputModel)
        {
            try
            {
                var updatedParticipant = await _context.SessionsParticipants.FindAsync(InputModel.SessionsParticipantsID);
                if (updatedParticipant != null)
                {
                    updatedParticipant.SessionId = InputModel.SessionID;
                    updatedParticipant.UserId = InputModel.UserID;
                    _context.SessionsParticipants.Update(updatedParticipant);

                    var saved = await _context.SaveChangesAsync();
                    return saved > 0;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while updating the session participant: " + ex.Message);
            }
        }
        //DONE
        public async Task<bool> DeleteUserFromSessionByUserID(int SessionID, int UserID)
        {
            try
            {
                var sessionParticipant = await _context.SessionsParticipants.Where(Sp=>Sp.UserId == UserID && Sp.SessionId == SessionID).ToListAsync();
                if (sessionParticipant.Any())
                {
                   _context.SessionsParticipants.RemoveRange(sessionParticipant);
                    var deleted = await _context.SaveChangesAsync();
                    return deleted > 0;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while deleting the session participant by ID: " + ex.Message);
            }
        }

        public async Task<bool> DeleteSessionsParticipantsByUser(int UserID)
        {
            try
            {
                var sessionParticipants = await _context.SessionsParticipants.Where(sp => sp.UserId == UserID).ToListAsync();
                if (sessionParticipants.Any())
                {
                    _context.SessionsParticipants.RemoveRange(sessionParticipants);
                    var deleted = await _context.SaveChangesAsync();
                    return deleted > 0;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while deleting the session participant by user: " + ex.Message);
            }
        }
        //DONE
        public async Task<bool> DeleteAllUserFromSessionByUserID(int SessionID)
        {
            try
            {
                var sessionParticipants = await _context.SessionsParticipants.Where(sp => sp.SessionId == SessionID).ToListAsync();
                if (sessionParticipants.Any())
                {
                    _context.SessionsParticipants.RemoveRange(sessionParticipants);
                    var deleted = await _context.SaveChangesAsync();
                    return deleted > 0;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while deleting the session participant by session: " + ex.Message);
            }
        }
  
        public async Task<SessionsParticipantsResponseModel?> GetSessionsParticipantsByUser(int UserID)
        {
            try
            {
                var sessionParticipant = await _context.SessionsParticipants.FirstOrDefaultAsync(sp => sp.UserId == UserID);
                if (sessionParticipant != null)
                {
                    return new SessionsParticipantsResponseModel
                    {
                        SessionsParticipantsID = sessionParticipant.ParticipateId,
                        SessionID = sessionParticipant.SessionId ?? 0,
                        UserID = sessionParticipant.UserId ?? 0
                    };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching session participant by user: " + ex.Message);
            }
        }
        //done
        public async Task<ActionResult<IEnumerable<SessionParticipantsUsers>>?> GetAllSessionsParticipantsByUser(int SessionID)
        {
            try
            {
                var sessionsParticipants = await _context.SessionsParticipants
                    .Where(sp => sp.SessionId == SessionID)
                    .ToListAsync();

                if (sessionsParticipants.Any())
                {
                    var userIds = sessionsParticipants.Select(sp => sp.UserId ?? 0).ToList();

                    var users = await _context.Users
                        .Where(u => userIds.Contains(u.UserId) && u.UserStatus == "User")
                        .Select(u => new SessionParticipantsUsers
                        {
                            UserID = u.UserId,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            UserEmail = u.UserEmail,
                            Phone = u.Phone,
                            City = u.City,
                            Gender = u.Gender
                        })
                        .ToListAsync();

                    return users;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while fetching session participants by user: " + ex.Message);
            }
        }
        //DONE
        public async Task<ActionResult<IEnumerable<SpeakerResponseModel>>?> GetAllSessionsParticipantsBySpeaker(int SessionID)
        {
            try
            {
                List<SpeakerResponseModel> speakers = new ();

                var sessionsParticipants = await _context.SessionsParticipants.Where(sp => sp.SessionId == SessionID).ToListAsync();

                foreach (var session in sessionsParticipants)
                {
                    var user = await _context.Users.FindAsync(session.UserId);
                    if (user != null)
                    {
                        var speaker = await _context.Speakers.FindAsync(user.UserId);
                        if (speaker != null)
                        {
                            var socialMedia = await _context.SocialMedia.FindAsync(speaker.SpeakerId);

                            SpeakerResponseModel speakerResponseModel = new SpeakerResponseModel
                            {
                                SpeakerID = speaker.SpeakerId,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                SpeakerEmail = user.UserEmail,
                                Phone = user.Phone,
                                City = user.City,
                                Gender = user.Gender,
                                Picture = speaker.Picture,
                                Mct = speaker.Mct,
                                Mvp = speaker.Mvp,
                                Biography = speaker.Biography,
                                Facebook = socialMedia?.Facebook,
                                Instagram = socialMedia?.Instagram,
                                LinkedIn = socialMedia?.LinkedIn,
                                Twitter = socialMedia?.Twitter,
                                Website = socialMedia?.Website
                            };

                            speakers.Add(speakerResponseModel);
                        }
                    }
                }

                return speakers;
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while fetching session participants by speaker: " + ex.Message);
            }
        }
    }
}
