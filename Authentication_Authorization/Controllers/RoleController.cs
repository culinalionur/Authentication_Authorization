using Authentication_Authorization.Models.DTOs;
using Authentication_Authorization.Models.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Authentication_Authorization.Controllers
{
    public class RoleController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View(_roleManager.Roles);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Required(ErrorMessage ="Please type into role name")][MinLength(3,ErrorMessage ="Minimum length is 3")]string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult identityResult = await _roleManager.CreateAsync(new IdentityRole(name));

                if (identityResult.Succeeded)
                {
                    TempData["Success"] = "The role has been created..!";
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach(IdentityError error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        TempData["Error"] = error.Description;                    }
                }
                
            }
            TempData["Error"] = "Something went wrong..";
            return View(name);
        }
        public async Task<IActionResult> AssignedUser(string Id)
        {
            IdentityRole identityRole = await _roleManager.FindByIdAsync(Id);

            List<AppUser> hasRole = new List<AppUser>();
            List<AppUser> hasNotRole = new List<AppUser>();

            foreach (AppUser user in _userManager.Users)
            {
                var list = await _userManager.IsInRoleAsync(user, identityRole.Name) ? hasRole : hasNotRole;
                list.Add(user);
            }
            AssignedRoleDTO assignedRoleDTO = new AssignedRoleDTO
            {
                Role = identityRole,
                HasRole = hasRole,
                HasNotRole = hasNotRole,
                RoleName = identityRole.Name
            };
            return View(assignedRoleDTO);
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignedUser(AssignedRoleDTO assignedRoleDTO)
        {
            IdentityResult result;

            foreach (var userId in assignedRoleDTO.AddIds ?? new string[] {})
            {
                AppUser appUser = await _userManager.FindByEmailAsync(userId);
                result = await _userManager.AddToRoleAsync(appUser, assignedRoleDTO.RoleName);
            }

            return RedirectToAction("Index");
        }
    }
}
