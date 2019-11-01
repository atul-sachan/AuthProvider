using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AuthProvider.Authentication.Models;
using Microsoft.AspNetCore.Identity;
using IdentityUser = AuthProvider.Authentication.Models.IdentityUser;
using IdentityRole = AuthProvider.Authentication.Models.IdentityRole;

namespace AuthProvider.Authentication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Register()
        {
            var vm = new RegisterModel()
            {
                Roles = roleManager.Roles.Select(x => new RoleCheckbox(x.Name)).ToList()
                //new List<RoleCheckbox>()
                //{
                //    new RoleCheckbox("User"),
                //    new RoleCheckbox("Admin"),
                //    new RoleCheckbox("SuperUser")
                //}
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var roles = model.Roles.Where(x => x.Selected == true).Select(x => x.NormalizeName).ToList();
                var user = await userManager.FindByNameAsync(model.UserName.ToUpper());

                if (user == null)
                {
                    user = new IdentityUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = model.UserName,
                        Email = model.Email,
                        PhoneNumber = model.Phone,
                        Roles = roles
                    };

                    var result = await userManager.CreateAsync(user, model.Password);
                }

                return View("Index");
            }

            return View();
        }

        [HttpGet]
        public IActionResult RegisterRoles()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterRoles(RegisterRoleModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByNameAsync(model.RoleName.ToUpper());

                if (role == null)
                {
                    role = new IdentityRole
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.RoleName
                    };

                    var result = await roleManager.CreateAsync(role);
                }

                return View("Index");
            }

            return View();
        }

    }
}
