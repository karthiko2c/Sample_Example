using RS.Data.Interfaces;
using RS.Data;
using RS.Entity;
using RS.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Collections.Generic;
using RS.ViewModel.SearchAndSortModel;

namespace RS.Data.Logic
{
    public class UserRepository : Repository<Users>, IUserRepository
    {
        private readonly RSContext _context;
        public UserRepository(RSContext context) : base(context)
        {
            this._context = context;
        }

        public void CreateUser(Users user, UserRoles userRole)
        {
            user.UserRoles.Add(userRole);
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public Users GetByID(Guid userId)
        {
            return _context.Users.Include(t => t.UserRoles).ThenInclude(r => r.Role).FirstOrDefault(x => (x.IsActive && !x.IsDeleted) && (x.UserId == userId));
        }

        public Users GetEmailIdByUserName(string userName)
        {
            return _context.Users.FirstOrDefault(x => x.UserName == userName && (x.IsActive && !x.IsDeleted));
        }

        public List<Users> GetUsersByRole(int roleId)
        {
            return _context.UserRoles.Where(x => x.RoleId == roleId && (x.IsActive && !x.IsDeleted)).Select(x => x.user).ToList();
        }

        public Users LoginUser(string username, string password)
        {
            return _context.Users.Include(t => t.UserRoles).ThenInclude(r => r.Role).FirstOrDefault(x => (x.IsActive && !x.IsDeleted) && (x.UserName == username.ToLower() && x.Password == password.ToLower()));
        }

        public Users GetUser(Guid userId, string password)
        {
            return _context.Users.Include(t => t.UserRoles).ThenInclude(r => r.Role).FirstOrDefault(x => (x.IsActive && !x.IsDeleted) && (x.UserId == userId && x.Password == password.ToLower()));
        }

        public void UpdateUserRole(UserRoles userRole)
        {
            _context.UserRoles.Add(userRole);
            _context.SaveChanges();
        }

        public void UpdateUser(Users user)
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
        }


        List<Users> IUserRepository.GetAll(SearchAndSortModel searchAndSortModel)
        {
            var userList = _context.Users.Include(t => t.UserRoles).ThenInclude(r => r.Role).Where(x => (x.IsActive && !x.IsDeleted)).ToList();

            if (searchAndSortModel.SearchString != null)
            {
                userList = userList.Where(x => x.FirstName.ToLower().Contains(searchAndSortModel.SearchString.ToLower()) ||
                    x.LastName.ToLower().Contains(searchAndSortModel.SearchString.ToLower()) ||
                    x.Email.ToLower().Contains(searchAndSortModel.SearchString.ToLower()) ||
                    x.UserRoles.Where(y => y.Role.Name.ToLower()
                    .Contains(searchAndSortModel.SearchString.ToLower())).Any()
                    ).ToList();
            }
            if (searchAndSortModel.Property != null)
            {
                if (searchAndSortModel.Direction == 1)
                {
                  userList = userList.OrderBy(x => x.GetType().GetProperty(searchAndSortModel.Property).GetValue(x, null)).ToList();
                }
                else
                {
                  userList = userList.OrderByDescending(x => x.GetType().GetProperty(searchAndSortModel.Property).GetValue(x, null)).ToList();
                }
            }
            return userList;
        }

    }
}
