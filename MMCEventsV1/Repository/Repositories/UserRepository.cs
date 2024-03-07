using MMCEventsV1.DTO.User;
using MMCEventsV1.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using MMCEventsV1.Repository.Models;
using ScaffoldConcept.TestModels;
using User = MMCEventsV1.Repository.Models.User;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MMCEventsV1.Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;



        private readonly MMC_Event _Context;
        private string? secretKey;
        public UserRepository(MMC_Event context, ILogger<UserRepository> logger, IConfiguration configuration)
        {
            _Context = context;
            _logger = logger;

            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
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
        public async Task<bool> DeleteUserAsycn(int UserID)//VERIFIED
        {
            try
            {
                var speakerToDelete = await _Context.Users.FindAsync(UserID);

                if (speakerToDelete == null)
                {
                    return false;
                }

                await _Context.Database.ExecuteSqlRawAsync("DELETE FROM Speakers WHERE SpeakerID = {0}", UserID);
                await _Context.Database.ExecuteSqlRawAsync("DELETE FROM SessionsParticipants WHERE USERID = {0}", UserID);
                _Context.Users.Remove(speakerToDelete);
                var created = await SaveAsync();

                return created;
            }
            catch (Exception ex)
            {
                return false;
            }


        }
        public async Task<LoginResponse> LogIn(LoginRequest userLogin) // VERIFIED
        {
            try
            {
                // Use await to asynchronously retrieve the user
                var userExist = await _Context.Users.FirstOrDefaultAsync(user => user.UserEmail.Equals(userLogin.UserEmail));

                if (userExist == null)
                {

                    return (null);
                }

                if (userExist.UserPassword == userLogin.UserPassword)
                {
 
                    var key = new byte[48]; 
                    using (var sha348 = SHA384.Create())
                    {
                        var hashedBytes = sha348.ComputeHash(Encoding.UTF8.GetBytes(secretKey));
                        Array.Copy(hashedBytes, key, Math.Min(key.Length, hashedBytes.Length));
                    }

                    var tokenDiscriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]{
                        new(ClaimTypes.Email, userExist.UserEmail),
                        new(ClaimTypes.Role, userExist.UserStatus),
                    }),
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha384Signature
                        )
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDiscriptor);
 
                    LoginResponse loginResponse = new()
                    {
                        UserID = userExist.UserId,
                        FirstName = userExist.FirstName,
                        LastName = userExist.LastName,
                        City = userExist.City,
                        Gender = userExist.Gender,
                        UserStatus = userExist.UserStatus,
                        Phone = userExist.Phone,
                        Token = tokenHandler.WriteToken(token),

                    };
                    return (loginResponse);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                 
                _logger.LogError(ex, "An error occurred during login.");
                return null;
            }
        }
        public async Task<bool> SaveAsync() //VERIFIED
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
        public async Task<bool> UpdateUserAsync(UserInputModel UpdateUser) // VERIFIED
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
        public async Task<bool> UserExistAsync(string? userEmail)//VERIFIED
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
        }
        public async Task<UserResponseModel> GetOneUser(int? UserID) //VERIFIED
        {
            try
            {
                var user = await _Context.Users.FindAsync(UserID);
                if (user == null)
                {
                    return null;
                }
                else
                {
                    UserResponseModel responseUser = new();
                    {
                        responseUser.Gender = user?.Gender;
                        responseUser.City = user.City;
                        responseUser.UserStatus = user.UserStatus;
                        responseUser.Phone = user.Phone;
                        responseUser.FirstName = user.FirstName;
                        responseUser.LastName = user.LastName;
                        responseUser.UserEmail = user.UserEmail;
                        responseUser.UserID = user.UserId;
                    }
                    return (responseUser);
                }



            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
