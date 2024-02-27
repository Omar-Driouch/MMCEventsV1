using Microsoft.AspNetCore.Mvc;
using MMCEventsV1.DTO.User;
using MMCEventsV1.Repository.Models;

namespace MMCEventsV1.Repository.Interfaces
{
    public interface IUserRepository
    {
        ICollection<UserResponseModel> GetUsers();

        bool IsUserUnique(string email);
        Task<bool> UserExistAsync(string? userEmail);
        Task<bool> CreateUser(AddUserModel User);
        Task<bool> UpdateUserAsync(UserInputModel User);
        Task<bool> DeleteUserAsycn(int userID);
        Task<bool> SaveAsync();
        Task<string> LogIn(LoginRequest userLogin);
        bool logout();
        
        Task<UserResponseModel> GetOneUser(int? UserID);


    }
}
