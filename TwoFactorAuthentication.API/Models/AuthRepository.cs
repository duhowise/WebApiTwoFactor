﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TwoFactorAuthentication.API.Models
{
    public class AuthRepository :IDisposable    
    {
        private AuthContext _ctx;
 
        private UserManager<ApplicationUser> _userManager;
 
        public AuthRepository()
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
        }
 
        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = userModel.UserName,
                TwoFactorEnabled = true,
                Psk = TwoStepsAuthenticator.Authenticator.GenerateKey() 
            };
 
            var result = await _userManager.CreateAsync(user, userModel.Password);
 
            return result;
        }
   public async Task<string> Totp(LoginModel loginModel)
        {
           
            var result = await _userManager.FindAsync(loginModel.UserName, loginModel.Password);
            var code=new TwoStepsAuthenticator.TimeAuthenticator();
            
        }
 
        public async Task<ApplicationUser> FindUser(string userName, string password)
        {
            var user = await _userManager.FindAsync(userName, password);
            
            return user;
        }
 
        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();
 
        }
    }

    public class LoginModel{
        
        
     [Required]
    [Display(Name = "User name")]
    public string UserName { get; set; }
 
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

        public string Code { get; set; }
    }
}