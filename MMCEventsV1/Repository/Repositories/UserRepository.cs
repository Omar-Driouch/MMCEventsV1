using MMCEventsV1.DTO.User;
using MMCEventsV1.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using MMCEventsV1.Repository.Models;
using ScaffoldConcept.TestModels;
using User = MMCEventsV1.Repository.Models.User;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MMCEventsV1.Repository.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly MMC_Event _Context;
        private string secretKey;
        public UserRepository(MMC_Event context)
        {
            _Context = context;
            // secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }
        public ICollection<UserResponseModel> GetUsers()// VERIFIED
        {
            try
            {
                var users = _Context.Users.OrderBy(p => p.UserId).ToList();

                if (users == null || !users.Any())
                {
                    return null;
                }

                var userResponseList = users.Select(user => new UserResponseModel
                {
                    UserID = user?.UserId,
                    FirstName = user?.FirstName,
                    LastName = user?.LastName,
                    UserEmail = user?.UserEmail,
                    Gender = user?.Gender,
                    UserPassword = user?.UserPassword,
                    City = user?.City,
                    UserStatus = user?.UserStatus,
                    Phone = user?.Phone
                }).ToList();

                return userResponseList;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching users.", ex);
            }

        }
        public async Task<bool> CreateUser(AddUserModel inputModel)//VERIFIED
        {
            try
            {
                User user = new()
                {
                    FirstName = inputModel.FirstName,
                    LastName = inputModel.LastName,
                    UserEmail = inputModel?.UserEmail,
                    UserPassword = inputModel?.UserPassword,
                    Phone = inputModel?.Phone,
                    City = inputModel.City,
                    Gender = inputModel.Gender
                };

                _Context.Users.Add(user);
                var check = await SaveAsync();
                return check;
            }
            catch (Exception ex)
            {

                return false;
            }
        }


        public async Task<bool> DeleteUserAsycn(int UserID)
        {
            try
            {
                var speakerToDelete = await _Context.Users.FindAsync(UserID);

                if (speakerToDelete == null)
                {
                    return false;
                }

                //await _Context.Database.ExecuteSqlRawAsync("DELETE FROM SessionsParticipants WHERE USERID = {0}", UserID);
                _Context.Users.Remove(speakerToDelete);
                var created = await SaveAsync();

                return created;
            }
            catch (Exception ex)
            {
                return false;
            }
         

    }


    public bool IsUserUnique(string email)
        {
            throw new NotImplementedException();
        }

        public bool loging(LoginRequest userlogin)
        {
            throw new NotImplementedException();
        }

        public bool logout()
        {
            throw new NotImplementedException();
        }

        public bool Register(RegisterRequest registerRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAsync() //done
        {
            try
            {
                var saved = await _Context.SaveChangesAsync();
                return saved > 0;
            }
            catch (DbUpdateException)
            {

                return false;
            }
        }
        public async Task<bool> UpdateUserAsync(UserInputModel UpdateUser) // done
        {
            try
            {

                var user = _Context.Users.Find(UpdateUser.UserID);
                if (user != null)
                {
                    user.UserId = (int)UpdateUser?.UserID;
                    user.FirstName = UpdateUser?.FirstName;
                    user.UserEmail = UpdateUser?.UserEmail;
                    user.LastName = UpdateUser?.LastName;
                    user.Phone = UpdateUser?.Phone;
                    user.City = UpdateUser?.City;
                    user.UserPassword = UpdateUser?.UserPassword;
                    user.Gender = UpdateUser?.Gender;

                    _Context.Users.Update(user);
                    var created = await SaveAsync();
                    return created;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> UserExistAsync(string? userEmail)
        {
            try
            {
                var userExist = await _Context.Users.FirstOrDefaultAsync(user => user.UserEmail.Equals(userEmail));
                if (userExist == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }



            }
            catch (Exception ex)
            {

                return true;
            }
        }//done


    }
}
