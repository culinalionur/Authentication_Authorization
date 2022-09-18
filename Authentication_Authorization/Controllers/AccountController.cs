using Authentication_Authorization.Models.DTOs;
using Authentication_Authorization.Models.Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Authentication_Authorization.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IPasswordHasher<AppUser> _passwordHasher;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IPasswordHasher<AppUser> passwordHasher)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _passwordHasher = passwordHasher;
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost,ValidateAntiForgeryToken,AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if(ModelState.IsValid)
            {
                AppUser appUser = new AppUser { UserName = model.UserName, Email = model.Email };
                IdentityResult identityResult = await _userManager.CreateAsync(appUser, model.Password);

                if (identityResult.Succeeded)
                    return View("Login");
                else
                    foreach (IdentityError error in identityResult.Errors)
                        ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult LogIn(string returnUrl)
        {
            return View(new LoginDTO { ReturnUrl = returnUrl }); 
        }
        [HttpPost,ValidateAntiForgeryToken,AllowAnonymous]
        public async Task<IActionResult> LogIn(LoginDTO model)
        {
            if(ModelState.IsValid)
            {
                AppUser appUser = await _userManager.FindByNameAsync(model.UserName);

                if (appUser != null)
                {

                    Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(appUser.UserName, model.Password,false,false);

                    if (signInResult.Succeeded) return Redirect(model.ReturnUrl ?? "/");
                    ModelState.AddModelError("", "Wrong creadation information..!");
                }
            
            }
            return View(model);
        }
        public async Task<IActionResult> Edit()
        {
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            UserUpdateDTO userUpdateDTO = new UserUpdateDTO(appUser);
            return View(userUpdateDTO);

        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserUpdateDTO userUpdateDTO)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = await _userManager.FindByEmailAsync(User.Identity.Name);
                appUser.UserName = userUpdateDTO.UserName;
                if (userUpdateDTO.Password != null)
                {
                    appUser.PasswordHash = _passwordHasher.HashPassword(appUser, userUpdateDTO.Password);
                }
                IdentityResult identityResult = await _userManager.UpdateAsync(appUser);
                if (identityResult.Succeeded)
                {
                    TempData["Success"] = "Your profile has been edited";
                }
                else
                {
                    TempData["Error"] = "Your profile hasn't been edited";
                }
            }
            return View(userUpdateDTO);
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
