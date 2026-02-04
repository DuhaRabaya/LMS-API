using LMS.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Utils
{
    public class UserSeedData : ISeedData
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserSeedData(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task DataSeed()
        {
            if (!await _userManager.Users.AnyAsync())
            {
                var user1 = new ApplicationUser
                {
                    UserName = "DRabaya",
                    Email = "duharabaya4@gmail.com",
                    FullName = "DuhaRabaya",
                    EmailConfirmed = true,
                };             

                var user2 = new ApplicationUser
                {
                    UserName = "Student",
                    Email = "studentemail@gmail.com",
                    FullName = "StudentName",
                    EmailConfirmed = true,
                };

                var user3 = new ApplicationUser
                {
                    UserName = "Instructor",
                    Email = "instructoremail@gmail.com",
                    FullName = "InstructorName",
                    EmailConfirmed = true,
                };

                await _userManager.CreateAsync(user1, "Pass#@1du");
                await _userManager.AddToRoleAsync(user1, "Admin");
                await _userManager.CreateAsync(user2, "Pass#@2st");
                await _userManager.AddToRoleAsync(user2, "Student");
                await _userManager.CreateAsync(user3, "Pass#@1in");
                await _userManager.AddToRoleAsync(user3, "Instructor");
            }
        }
    }
}
