using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Models.Identity;

namespace Talabat.Apis.Extentions
{
    public static class UserManagerExtention
    {
        public static async Task<AppUser?> FindByAddressAsync(this UserManager<AppUser> usermanager,ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await usermanager.Users.Include(U=>U.Address).FirstOrDefaultAsync(U=>U.Email==email);
            return user;

        }
    }
}
