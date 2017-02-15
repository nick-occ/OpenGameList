﻿using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using OpenGameList.Data;
using OpenGameList.Data.Users;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OpenGameListWebApp.Controllers
{
    [Route("api/[controller]")]
    public class BaseController : Controller
    {
        #region Common Fields
        protected ApplicationDbContext DbContext;
        protected SignInManager<ApplicationUser> SignInManager;
        protected UserManager<ApplicationUser> UserManager;
        #endregion Common Fields

        #region Constructor
        public BaseController(ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            //Dependency Injection
            DbContext = context;
            SignInManager = signInManager;
            UserManager = userManager;
        }
        #endregion Constructor

        #region Common Methods
        /// <summary>
        /// Retrieves the .NET Core Identity User Id
        /// for the current ClaimsPrincipal.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetCurrentUserId()
        {
            //if the user is not authenticated, throw an exception
            if (!User.Identity.IsAuthenticated)
                throw new NotSupportedException();

            var info = await SignInManager.GetExternalLoginInfoAsync();
            if (info == null)
                //internal provider
                return User.FindFirst(ClaimTypes.NameIdentifier).Value;
            else
            {
                //external provider
                var user = await UserManager.FindByLoginAsync(
                    info.LoginProvider,
                    info.ProviderKey);
                if (user == null) throw new NotSupportedException();
                return user.Id;
            }

        }
        #endregion Common Methods

        #region Common Properties
        /// <summary>
        /// Returns a suitable JsonSerializerSettings object  
        /// that can be used to generate the JsonResult return value  
        /// for this Controller's methods. 
        /// </summary>
        protected JsonSerializerSettings DefaultJsonSettings
        {
            get
            {
                return new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                };
            }
        }
        #endregion Common Properties
    }
}
