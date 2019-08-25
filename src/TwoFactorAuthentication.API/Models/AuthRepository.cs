using System;
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
                Psk = TimeSensitivePassCode.GeneratePresharedKey()
            };
 
            var result = await _userManager.CreateAsync(user, userModel.Password);
 
            return result;
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
}