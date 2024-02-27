using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMCEventsV1.DTO.User;
using MMCEventsV1.Repository;
using MMCEventsV1.Repository.Interfaces;
using MMCEventsV1.Repository.Models;
using MMCEventsV1.Repository.Repositories;
using Newtonsoft.Json;
using ScaffoldConcept.TestModels;
using System.Numerics;
using System.Reflection;
using User = MMCEventsV1.Repository.Models.User;




namespace MMCEventsV1.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserResponseModel>))] // VERIFIED
        //[Authorize(Roles = "admin")]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers();
            if (users == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(users);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateUserAsync(AddUserModel userCreate) //Verified
        {
            var isUserExist = await _userRepository.UserExistAsync(userCreate.UserEmail);
            if (isUserExist == false)
            {
                var created = await _userRepository.CreateUser(userCreate);
                if (created)
                {
                    return Ok(created);
                }
                else
                { return BadRequest("An error occured "); }
            }
            else
            {
                return BadRequest(new { message = "User already exists", errorCode = "USER_ALREADY_EXISTS" });

            }

        }


        [HttpPut("{UserID}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateUserAsync(UserInputModel NewUser)
        {
            var check = await _userRepository.UpdateUserAsync(NewUser);
            if(check)
            {
                return Ok("User has been updated successfylly");
            }
            else
            { return BadRequest("An error occured "); }
        }



        // DELETE api/<UserController>/ Verified 
        [HttpDelete("{UserID}")]
        public async Task<IActionResult> DeleteUserAsycn(int UserID)
        {
            var check = await _userRepository.DeleteUserAsycn(UserID);
            if (check)
            {
                return Ok("User has been Deleted successfylly");
            }
            else
            { return BadRequest("An error occured "); }

        }




    }
}
