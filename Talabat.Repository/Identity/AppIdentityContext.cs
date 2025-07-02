using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Identity;

namespace Talabat.Repository.Identity
{
     public class AppIdentityContext : IdentityDbContext<AppUser>
    {
         public AppIdentityContext(DbContextOptions options) : base(options)
        {
        }

    }
}
