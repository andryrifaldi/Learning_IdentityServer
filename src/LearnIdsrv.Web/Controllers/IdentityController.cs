using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace LearnIdsrv.Web.Controllers
{
    public class IdentityController : Controller
    {
        public IActionResult Login()
        {
            return Challenge(new Microsoft.AspNetCore.Authentication.AuthenticationProperties() { RedirectUri = "Home/Index"}, Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectDefaults.AuthenticationScheme);
        }

        public ActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}
