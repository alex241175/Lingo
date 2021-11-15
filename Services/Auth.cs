using Microsoft.AspNetCore.Http;
using Lingo.Models;
using Newtonsoft.Json;

namespace Lingo.Services
{
    public class Auth
    {
        User User = new User{};
        public bool IsAdmin = false;
        public bool IsLogin = false;
        public int UserId;
        public string UserName;
        
        public Auth(IHttpContextAccessor accessor){
            string user = accessor.HttpContext.Session.GetString("User");
            IsLogin = user != null ;
            if (IsLogin){
                User = JsonConvert.DeserializeObject<User>(user);
                IsAdmin = User.Role == "Admin";
                UserId = User.UserId;
                UserName = User.UserName;
            }        
        }
    } 
}