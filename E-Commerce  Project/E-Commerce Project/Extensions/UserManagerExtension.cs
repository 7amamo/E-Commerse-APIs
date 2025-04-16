using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Project.Extensions
{
    public static class UserManagerExtension
    {
        public async static Task<AppUser?> FindUserWithAddressAsync(this UserManager<AppUser> userManager, string email)
        {
            var user = await userManager.Users.Include(A => A.Address).FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }
    }
}
