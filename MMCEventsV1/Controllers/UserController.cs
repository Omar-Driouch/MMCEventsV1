using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMCEventsV1.DTO;
using MMCEventsV1.Repository;
using MMCEventsV1.Repository.Models;
using Newtonsoft.Json;
using ScaffoldConcept.TestModels;
using System.Numerics;
using System.Reflection;
using User = MMCEventsV1.Repository.Models.User;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MMCEventsV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
      
        private readonly MMC_Event _User;
        private readonly MMC_Event _SessionsParticipants;

        public UserController(MMC_Event context) { _User = context; _SessionsParticipants = context; }

        // GET All the Users: api/<UserController>/ verified
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInputModel>>> Get()
        {
            try
            {
                var users = _User.Users.ToList();
                if(users==null)
                {
                    return NotFound();
                }
                List<UserResponseModel> userResponseList = new List<UserResponseModel>();

                foreach (var user in users)
                {
                    if (user .UserStatus=="User")// only show the users 
                    {

                        UserResponseModel userResponseModel = new UserResponseModel
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
                        };

                        userResponseList.Add(userResponseModel);

                    }
                }

                return Ok(userResponseList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET All the Users: api/<UserController>/ verifeid
        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<UserInputModel>>> GetAll()
        {
            var users = _User.Users.ToList();
            if (users == null)
            {
                return NotFound();
            }
            List<UserResponseModel> userResponseList = new List<UserResponseModel>();

            foreach (var user in users)
            {

                UserResponseModel userResponseModel = new UserResponseModel
                {
                        UserID = user?.UserId,
                        FirstName = user?.FirstName,
                        LastName = user?.LastName,
                        UserEmail = user?.UserEmail,
                        Gender = user?.Gender,
                        City = user?.City,
                        UserPassword = user?.UserPassword,
                        UserStatus = user?.UserStatus,
                        Phone = user?.Phone
                    };

                    userResponseList.Add(userResponseModel);
              
            }

            return Ok(userResponseList);
        }

        // GET  UserIBy ID api/<UserController> // Verifed
        [HttpGet("{UserID}")]
        public async Task<ActionResult<UserResponseModel>> GetUserByID(int UserID)
        {
            var user = _User.Users.Find(UserID);
            UserResponseModel inputModel = new();
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                inputModel.Gender = user.Gender;
                inputModel.City = user.City;
                inputModel.UserStatus = user.UserStatus;
                inputModel.Phone = user.Phone;
                inputModel.FirstName = user.FirstName;
                inputModel.LastName = user.LastName;
                inputModel.UserEmail = user.UserEmail;
                inputModel.UserID = user.UserId;


                return Ok(inputModel);
            }
        }

        // POST api/<UserController> Verified 
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] AddUserModel inputModel)
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
            _User.Users.Add(user);
            await _User.SaveChangesAsync();

            //return Ok($"The User added Successfully: {JsonConvert.SerializeObject(inputModel)}");
            return Ok("The User added Successfully");

        }

        // PUT api/<UserController>/ Verified
        [HttpPut("{UserID}")]
        public async Task<IActionResult> UpdateUser([FromBody] UserInputModel inputModel)
        {
            try
            {
          
                var user = _User.Users.Find(inputModel.UserID);
                if (user != null)
                {
                    user.UserId =(int) inputModel?.UserID;
                    user.FirstName = inputModel?.FirstName;
                    user.UserEmail = inputModel?.UserEmail;
                    user.LastName = inputModel?.LastName;
                    user.Phone = inputModel?.Phone;
                    user.City = inputModel?.City;
                    user.UserPassword = inputModel?.UserPassword;
                    user.Gender = inputModel?.Gender;


                    _User.Users.Update(user);
                    await _User.SaveChangesAsync();
                    return Ok(inputModel);
                }
                else
                {
                    return NotFound("This Speaker not found" + inputModel.UserID);
                }




            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        // DELETE api/<UserController>/ Verified 
        [HttpDelete("{UserID}")]
        public async Task<ActionResult<string>> DeleteSpeaker(int UserID)
        {
            try
            {
                var speakerToDelete = await _User.Users.FindAsync(UserID);

                if (speakerToDelete == null)
                {
                    return NotFound("User not found");
                }

                await _User.Database.ExecuteSqlRawAsync("DELETE FROM SessionsParticipants WHERE USERID = {0}", UserID);
               _User.Users.Remove(speakerToDelete);
                await _User.SaveChangesAsync();

                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
