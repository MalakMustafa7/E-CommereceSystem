﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Identity;

namespace Talabat.Core.Services
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync(AppUser User , UserManager<AppUser> userManager);

    }
}
