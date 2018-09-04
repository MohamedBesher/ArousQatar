

using System;

namespace Saned.ArousQatar.Data.Core.Dtos
{
   
    public class UserProfileDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhotoUrl { get; set; }
        public string PhoneNumber { get; set; }
        public int AdsCount { get; set; }
        
    }
    public class RegisterData
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class RegisterUserData : RegisterData
    {
        public bool Gender { get; set; }
        public DateTime BirthDay { get; set; }
        public string SoicalMediaId { get; set; }

    }


}