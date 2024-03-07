﻿namespace MMCEventsV1.DTO.User
{
    public class RegisterRequest
    {
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Gender { get; set; }
        public string? Phone { get; set; }
        public string? City { get; set; }

    }
}
