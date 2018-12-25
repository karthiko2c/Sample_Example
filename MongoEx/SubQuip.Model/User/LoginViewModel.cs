using System;
using System.ComponentModel.DataAnnotations;

namespace SubQuip.ViewModel.User
{
    public class UserLoginViewModel
    {
        public string UserName { get; set; }
        public string UserPassword { get; set; }
    }
}
