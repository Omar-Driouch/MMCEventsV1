﻿namespace MMCEventsV1.DTO.User
{
    public class LoginResponse
    {
        public int? UserID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }


       // public string? UserEmail { get; set; }
      

        public string? Phone { get; set; }
        public string? City { get; set; }
        public string? Gender { get; set; }

        public string? UserStatus { get; set; }
        public string Token { get; set; }
    }
}
