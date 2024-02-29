using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MMCEventsV1.DTO.Events;
using MMCEventsV1.Repository.Interfaces;
using MMCEventsV1.Repository.Models;

namespace MMCEventsV1.Repository.Repositories
{
    public class EventsRepository : IEventsRepository
    {
        private readonly MMC_Event _Event;

        public EventsRepository(MMC_Event events)
        {
            _Event = events;
        }
        public async Task<ActionResult<IEnumerable<EventInputModel>>> GetEvents() // DONE
        {
            try
            {
                var events = _Event.Events.ToList();
                if (events == null)
                {
                    return null;
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

                return (eventResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Events ");
            }
        }

        public async Task<ActionResult<EventResponseModel>> GetEventById(int EventID) //DONE 
        {
            try
            {
                var ev = await _Event.Events.FindAsync(EventID);
                if (ev == null)
                {
                    return null;
                }

                EventResponseModel eventResponseModel = new EventResponseModel
                {
                    EventID = ev.EventId,
                    Title = ev.Title,
                    Description = ev?.Description,
                    Picture = ev?.Picture,
                    StartDate = (DateTime)ev?.StartDate,
                    EndDate = (DateTime)ev?.EndDate

                };

                return (eventResponseModel);
            }
            catch (Exception)
            {

                throw new Exception("Erorr while fetching the event");
            }
        }

        public async Task<bool> CreateEvent(EventUpdateModel eventUpdateModel) //DONE
        {
            try
            {
                var ev = new Event
                {
                    Title = eventUpdateModel?.Title,
                    Description = eventUpdateModel?.Description,
                    Picture = eventUpdateModel?.Picture,
                    StartDate = eventUpdateModel?.StartDate,
                    EndDate = eventUpdateModel?.EndDate
                };

                await _Event.Events.AddAsync(ev); // Await the AddAsync method call

                var saved = await _Event.SaveChangesAsync();

                return saved > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Internal server error: " + ex.Message);
            }
        }
        public async Task<bool> UpdateEvent(EventInputModel inputModel)//DONE
        {
            try
            {
                var findEvent = await _Event.Events.FindAsync(inputModel.EventID);
                if (findEvent != null)
                {
                    findEvent.Title = inputModel.Title;
                    findEvent.Description = inputModel.Description;
                    findEvent.Picture = inputModel.Picture;
                    findEvent.StartDate = inputModel.StartDate;
                    findEvent.EndDate = inputModel.EndDate;

                    _Event.Events.Update(findEvent);
                    var saved = await _Event.SaveChangesAsync();
                    return saved > 0;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Error while Updating the Events");
            }

        }

        public async Task<bool> DeleteEvent(int EventID)//DONE
        {
            try
            {
                var findEvent = await _Event.Events.FindAsync(EventID);
                if (findEvent != null)
                {
                   
                        //--- find and Delete all the event from EventPartner Table ---
                        var evtPartnersToDelete = await _Event.EventPartners
                                            .Where(ep => ep.EventId == EventID)
                                            .ToListAsync();
                        foreach (var item in evtPartnersToDelete)
                        {
                            _Event.EventPartners.Remove(item);
                            await _Event.SaveChangesAsync();
                        }

                        //-------------------------------------------------------
                   
                        //--- find and Delete all the SupportSession from SupportSession Table  ---
                        var SessionToDelete = await _Event.Sessions
                                            .Where(ep => ep.EventId == EventID)
                                            .ToListAsync();
                        foreach (var item in SessionToDelete)
                        {
                            var SupportSessionToDelete = await _Event.SupportSessions
                                           .Where(ep => ep.SessionId == item.SessionId)
                                           .ToListAsync();
                            foreach (var sprt in SupportSessionToDelete)
                            {
                                _Event.SupportSessions.Remove(sprt);
                                await _Event.SaveChangesAsync();
                            }
                            //--- find and Delete all the SposnorSession from SposnorSession Table  ---
                            var SposnorSessionList = await _Event.SponsorSessions
                                                .Where(sps => sps.SessionId == item.SessionId)
                                                .ToListAsync();
                            foreach (var SponsorSessionToDelete in SposnorSessionList)
                            {
                                _Event.SponsorSessions.Remove(SponsorSessionToDelete);
                                await _Event.SaveChangesAsync();
                            }
                        }
                        //-------------------------------------------------------
                   

                    _Event.Remove(findEvent);
                    var saved = await _Event.SaveChangesAsync();
                    return saved > 0;
                }
                else { return false; }
            }
            catch (Exception)
            {

                throw new Exception("Error while deleting the event");
            }
        }


    }
}
