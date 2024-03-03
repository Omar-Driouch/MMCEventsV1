using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMCEventsV1.DTO.EventPartner;
using MMCEventsV1.Repository.Interfaces;
using MMCEventsV1.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMCEventsV1.Repository.Repositories
{
    public class EventPartnerRepository : IEventPartnerRepo
    {
        private readonly MMC_Event _context;

        public EventPartnerRepository(MMC_Event context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<EventPartnerResponseModel>>> GetAllEventPartners()
        {
            try
            {
                var eventPartners = await _context.EventPartners.ToListAsync();

                var result = eventPartners.Select(item => new EventPartnerResponseModel
                {
                    EventPartnerID = item.EventPartnerId,
                    PartnerID = item.EventId ?? 0,
                    EventID = item.EventId ?? 0,
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while fetching event partners: " + ex.Message);
            }
        }

        public async Task<bool> AddNewEventPartner(EventPartnerInputModel inputModel)
        {
            try
            {
                var eventPartner = new EventPartner
                {
                    PartnerId = inputModel.PartnerID,
                    EventId = inputModel.EventID
                };

                await _context.EventPartners.AddAsync(eventPartner);
                var saved = await _context.SaveChangesAsync();

                return saved > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding a new event partner: " + ex.Message);
            }
        }

        public async Task<bool> UpdateEventPartner(EventPartnerResponseModel inputModel)
        {
            try
            {
                var updatedEventPartner = await _context.EventPartners.FindAsync(inputModel.EventPartnerID);
                if (updatedEventPartner != null)
                {
                    updatedEventPartner.PartnerId = inputModel.PartnerID;
                    updatedEventPartner.EventId = inputModel.EventID;

                    _context.EventPartners.Update(updatedEventPartner);

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
                throw new Exception("Error occurred while updating the event partner: " + ex.Message);
            }
        }

        public async Task<bool> DeleteEventPartner(int eventPartnerId)
        {
            try
            {
                var eventPartner = await _context.EventPartners.FindAsync(eventPartnerId);
                if (eventPartner != null)
                {
                    _context.EventPartners.Remove(eventPartner);
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
                throw new Exception("Error occurred while deleting the event partner: " + ex.Message);
            }
        }
    }
}
