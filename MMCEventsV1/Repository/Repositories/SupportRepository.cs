using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMCEventsV1.DTO.Support;
using MMCEventsV1.Repository.Interfaces;
using MMCEventsV1.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMCEventsV1.Repository.Repositories
{
    public class SupportRepository : ISupportRepository
    {
        private readonly MMC_Event _SupportContext;

        public SupportRepository(MMC_Event supportContext)
        {
            _SupportContext = supportContext;
        }
        //done
        public async Task<ActionResult<IEnumerable<SupportResponseModel>>?> GetAll()
        {
            try
            {
                var supports = await _SupportContext.Supports.ToListAsync();

                if (supports.Any())
                {
                    var result = supports.Select(item => new SupportResponseModel
                    {
                        SupportId = item.SupportId,
                        Name = item.Name,
                        Duration = item.Duration,
                        Path = item.Path,
                        Status = item.Status
                    }).ToList();

                    return result;
                }
                else
                {
                    return new List<SupportResponseModel>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while fetching supports: " + ex.Message);
            }
        }
        //done
        public async Task<bool> AddNewSupport(SupportInputModel supportInputModel)
        {
            try
            {
                Support support = new()
                {
                    Name = supportInputModel.Name,
                    Duration = supportInputModel.Duration,
                    Path = supportInputModel.Path,
                    Status = supportInputModel.Status
                };

                _SupportContext.Add(support);
               var saved =  await _SupportContext.SaveChangesAsync();

                return saved > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding a new support: " + ex.Message);
            }
        }
        //done
        public async Task<bool> UpdateSupport(SupportResponseModel supportInputModel)
        {
            try
            {
                var updatedSupport = await _SupportContext.Supports.FindAsync(supportInputModel.SupportId);
                if (updatedSupport != null)
                {
                    updatedSupport.Name = supportInputModel.Name;
                    updatedSupport.Duration = supportInputModel.Duration;
                    updatedSupport.Path = supportInputModel.Path;
                    updatedSupport.Status = supportInputModel.Status;

                    _SupportContext.Supports.Update(updatedSupport);
                    var saved = await _SupportContext.SaveChangesAsync();
                    return saved > 0;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while updating the support: " + ex.Message);
            }
        }
        // done
        public async Task<bool> DeleteSupport(int SupportID)
        {
            try
            {
                var supportToDelete = await _SupportContext.Supports.FindAsync(SupportID);
                
                if (supportToDelete != null)
                {
                    var supporSession = _SupportContext.SupportSessions.Where(s => s.SupportId == SupportID).ToList();
                    if (supporSession.Count > 0)
                    {
                        _SupportContext.SupportSessions.RemoveRange(supporSession);
                    }
                    _SupportContext.Supports.Remove(supportToDelete);
                    var saved = await _SupportContext.SaveChangesAsync();
                    return saved > 0;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while deleting the support: " + ex.Message);
            }
        }
        // done
        public async Task<SupportResponseModel?> GetSupportById(int SupportID)
        {
            try
            {
                var support = await _SupportContext.Supports.FindAsync(SupportID);
                if (support != null)
                {
                    SupportResponseModel supportResponseModel = new()
                    {
                        SupportId = support.SupportId,
                        Name = support.Name,
                        Duration = support.Duration,
                        Path = support.Path,
                        Status = support.Status
                    };
                    return supportResponseModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching the support: " + ex.Message);
            }
        }
    }
}
