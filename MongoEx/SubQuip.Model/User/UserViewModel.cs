using System;
using System.Collections.Generic;

namespace SubQuip.ViewModel.User
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public bool IsAdmin { get; set; }
        public string Company { get; set; }
        public string Token { get; set; }
    }
}
