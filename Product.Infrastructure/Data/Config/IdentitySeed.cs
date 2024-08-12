using Microsoft.AspNetCore.Identity;
using Product.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure.Data.Config
{
    public class IdentitySeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "DoanTam",
                    Email = "doantampn2017@gmail.com",
                    UserName="Anh3",
                    Address = new Address
                    {
                        FirstName="Doan",
                        LastName="Tam",
                        City="HCM",
                        State="Q8",
                        Street="PhamHung",
                        Zipcode= "70000"
                    }
                };
                await userManager.CreateAsync(user, "Admin@123");
            }
        }
    }
}
