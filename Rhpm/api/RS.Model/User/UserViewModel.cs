using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;

namespace RS.ViewModel.User
{
    public class UserViewModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public int RoleId { get; set; }
        public ActionResult ProfileImage { get; set; }
        public Dictionary<String , List<int>> ApprovalDetail { get; set; }
    }
}
