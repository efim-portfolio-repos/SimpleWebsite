using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SimpleWebsite.Models;
using SimpleWebsite.Models.ViewModels;

namespace SimpleWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admins")]
    public class UserController : Controller
    {
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public ViewResult Index()
        {
            return View(_userManager.Users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            User editableUser = await _userManager.FindByIdAsync(id);
            if (editableUser != null)
            {
                ViewBag.IsAdmin = await _userManager.IsInRoleAsync(editableUser, "Admins");
                ViewBag.UserName = editableUser.UserName;
                Dictionary<string, bool> roles = _roleManager.Roles.Select(r => r.Name).ToDictionary(r => r, r => false);

                foreach (var r in await _userManager.GetRolesAsync(editableUser))
                {
                    roles[r] = true;
                }   

                return View(roles);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, IEnumerable<string> inputRoles)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                List<string> allRoles = _roleManager.Roles.Select(r => r.Name).ToList();
                List<string> userRoles = (await _userManager.GetRolesAsync(user)).ToList();

                List<string> filteredRoles = allRoles.Intersect(inputRoles).ToList();

                List<string> rolesToDelete = userRoles.Except(filteredRoles).ToList();
                rolesToDelete.Remove("Admins");
                List<string> rolesToAdd = filteredRoles.Except(userRoles).ToList();

                await _userManager.RemoveFromRolesAsync(user, rolesToDelete);
                await _userManager.AddToRolesAsync(user, rolesToAdd);
                await _userManager.UpdateSecurityStampAsync(user);
            }


            return RedirectToAction(nameof(Index));
        }
    }
}