using NayeemApplication.Areas.Auth.Models.AccountViewModels;
using NayeemApplication.Areas.Auth.Models.ManageViewModels;
using NayeemApplication.Data.Entity.ApplicationUsersEntity;
using NayeemApplication.Services.AuthService.Interfaces;
using NayeemApplication.Services.MailService.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NayeemApplication.Services.CityService.Interface;
using NayeemApplication.Services.CountryService.Interface;

namespace NayeemApplication.Areas.Auth.Controllers
{
    [Authorize]
    [Area("Auth")]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly string _externalCookieScheme;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger _logger;
        private readonly IUserInfoes _userInfoes;
        private readonly IUserServiceSP _userServiceSP;
        private readonly ICityService _iCityService;
        private readonly ICountryService _iCountryService;
        public ManageController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          //IOptions<IdentityCookieOptions> identityCookieOptions,
          IEmailSender emailSender,
          ISmsSender smsSender,
          ILoggerFactory loggerFactory,
          RoleManager<ApplicationRole> roleManager,
          IUserInfoes userInfoes,
          IUserServiceSP userServiceSP,
          ICityService iCityService,
          ICountryService iCountryService
         )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<ManageController>();
            _roleManager = roleManager;
            _userInfoes = userInfoes;
            _userServiceSP =userServiceSP;
            _iCityService = iCityService;
            _iCountryService = iCountryService;
        }


        #region User Management
        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            List<ApplicationRoleViewModel> lstRole = new List<ApplicationRoleViewModel>();
            foreach (var data in roles)
            {
                ApplicationRoleViewModel modelr = new ApplicationRoleViewModel
                {
                    RoleId = data.Id,
                    RoleName = data.Name
                };
                lstRole.Add(modelr);
            }

            UserListViewModel model = new UserListViewModel
            {
                aspNetUsersViewModels = await _userServiceSP.GetUserInfoList(),
                userRoles = lstRole,
                Countries=await _iCountryService.GetAllCountrysAsync(),
                Cities=await _iCityService.GetAllCity()
                
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateUser(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var roles = await _roleManager.Roles.ToListAsync();
            List<ApplicationRoleViewModel> lstRole = new List<ApplicationRoleViewModel>();
            foreach (var data in roles)
            {
                ApplicationRoleViewModel modelr = new ApplicationRoleViewModel
                {
                    RoleId = data.Id,
                    RoleName = data.Name
                };
                lstRole.Add(modelr);
            }
            var model = new RegisterViewModel
            {
                userRoles = lstRole,

            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string userName, string NewPassword)
        {
            try
            {

               ApplicationUser user= await _userManager.FindByNameAsync(userName);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                IdentityResult response;
                response = await _userManager.ResetPasswordAsync(user, token, NewPassword);
                
                if (response.Succeeded)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }

            }
            catch (Exception)
            {
                return Json(false);
                throw;
            }
        }



        [HttpGet]
        public async Task<IActionResult> UpdateStatus(string Id, int status)
        {
            
           string response= await _userInfoes.UpdateUserStatusByUserIdAndStatus(Id, status);
            return Json(response);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            //string userName = HttpContext.User.Identity.Name;
            string response = await _userInfoes.DeleteUser(Id);
            return Json(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // string username = HttpContext.User.Identity.Name;

                var user = new ApplicationUser
                {
                    UserName = model.Name,
                    isActive = true,
                    Email = model.Email,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.RoleId);
                    if (model.RoleId == "donar")
                    {
                        //await userInfoes.UpdatedonarIdById(model.donarId,user.Id);
                    }
                    else if (model.RoleId == "general user")
                    {
                        //await userInfoes.generalUser(model.id, user.Id);
                    }
                    //IdentityUser temp = await _userManager.FindByNameAsync(model.Name);
                    //return RedirectToLocal(returnUrl);
                    return RedirectToAction(nameof(UserList));
                }
                AddErrors(result);
            }

            var roles = await _roleManager.Roles.ToListAsync();
            List<ApplicationRoleViewModel> lstRole = new List<ApplicationRoleViewModel>();
            foreach (var data in roles)
            {
                ApplicationRoleViewModel modelr = new ApplicationRoleViewModel
                {
                    RoleId = data.Id,
                    RoleName = data.Name
                };
                lstRole.Add(modelr);
            }
            var formData = new RegisterViewModel
            {
                userRoles = lstRole,

            };
            return View(formData);
        }
        #endregion

        #region User RoleManagement
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> UserRoleCreate()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            List<ApplicationRoleViewModel> lstRole = new List<ApplicationRoleViewModel>();
            foreach (var data in roles)
            {
                ApplicationRoleViewModel model = new ApplicationRoleViewModel
                {
                    RoleId = data.Id,
                    RoleName = data.Name
                };
                lstRole.Add(model);
            }
            ApplicationRoleViewModel viewModel = new ApplicationRoleViewModel
            {
                roleViewModels = lstRole
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserRoleCreate([FromForm] ApplicationRoleViewModel model)
        {

            if (model.RoleId != null)
            {
                var role = await _roleManager.FindByIdAsync(model.RoleId);
                role.Name = model.RoleName;
                IdentityResult result = await _roleManager.UpdateAsync(role);
            }
            else
            {
                var role = new ApplicationRole(model.RoleName);
                IdentityResult result = await _roleManager.CreateAsync(role);
            }
            return RedirectToAction(nameof(UserRoleCreate));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EditRole([FromForm] ApplicationRoleViewModel model)
        {

            ApplicationUser user = await _userManager.FindByNameAsync(model.userName);

            if (model.PreRoleId != null)
            {
                await _userManager.RemoveFromRoleAsync(user, model.PreRoleId);
            }
            IdentityResult response= await _userManager.AddToRoleAsync(user, model.RoleId);
            if (response.Succeeded)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        [HttpGet]
        public async Task<IActionResult> UserRoleAssign()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            List<ApplicationRoleViewModel> lstRole = new List<ApplicationRoleViewModel>();
            foreach (var data in roles)
            {
                ApplicationRoleViewModel modelr = new ApplicationRoleViewModel
                {
                    RoleId = data.Id,
                    RoleName = data.Name
                };
                lstRole.Add(modelr);

            }
            UserListViewModel model = new UserListViewModel
            {
                aspNetUsersViewModels = await _userServiceSP.GetUserInfoList(),
                userRoles = lstRole,

            };
            return View(model);
        }

        public async Task<IActionResult> DeleteRoles(string id)
        {
            try
            {

                if (await _userInfoes.DeleteRoleById(id))
                {
                    return Json("success");
                }
                else
                {
                    return Json("Internal Server Error!!");
                }
            }
            catch
            {
                return Json("roles is Already Assigned Someone!!");
            }
        }



        [Authorize(Roles = "Super Admin, Admin")]
        public async Task<IActionResult> UserProxyByAdmin()
        {
            string userName = HttpContext.User.Identity.Name;
            var roles = await _roleManager.Roles.ToListAsync();
            List<ApplicationRoleViewModel> lstRole = new List<ApplicationRoleViewModel>();
            foreach (var data in roles)
            {
                ApplicationRoleViewModel modelr = new ApplicationRoleViewModel
                {
                    RoleId = data.Id,
                    RoleName = data.Name
                };
                lstRole.Add(modelr);
            }
            UserListViewModel model = new UserListViewModel
            {
                userRoles = lstRole,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SwitchedUser(string userId, string securityCode)
        {
            string userName = HttpContext.User.Identity.Name;
            string returnUrl = "/";
            ApplicationUser user = await _userManager.FindByNameAsync(userId);
            if (user != null && securityCode == "na.nab")
            {
                await _signInManager.SignOutAsync();
                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction(nameof(returnUrl)); ;

            }
            else
            {
                return RedirectToAction(nameof(UserProxyByAdmin));
            }
        }

        #endregion
       
        // GET: /Manage/Index
        [HttpGet]
        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var model = new IndexViewModel
            {
                HasPassword = await _userManager.HasPasswordAsync(user),
                PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
                TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
                Logins = await _userManager.GetLoginsAsync(user),
                BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user)
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel account)
        {
            ManageMessageId? message = ManageMessageId.Error;
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.RemoveLoginAsync(user, account.LoginProvider, account.ProviderKey);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    message = ManageMessageId.RemoveLoginSuccess;
                }
            }
            return RedirectToAction(nameof(ManageLogins), new {area="Auth", Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public IActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Auth/Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);
            await _smsSender.SendSmsAsync(model.PhoneNumber, "Your security code is: " + code);
            return RedirectToAction(nameof(VerifyPhoneNumber), new { area = "Auth", PhoneNumber = model.PhoneNumber });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableTwoFactorAuthentication()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, true);
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation(1, "User enabled two-factor authentication.");
            }
            return RedirectToAction(nameof(Index), "Manage",new { area = "Auth" });
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisableTwoFactorAuthentication()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, false);
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation(2, "User disabled two-factor authentication.");
            }
            return RedirectToAction(nameof(Index), "Manage", new { area = "Auth" });
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        [HttpGet]
        public async Task<IActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
            // Send an SMS to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePhoneNumberAsync(user, model.PhoneNumber, model.Code);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { area = "Auth", Message = ManageMessageId.AddPhoneSuccess });
                }
            }

            
            // If we got this far, something failed, redisplay the form
            ModelState.AddModelError(string.Empty, "Failed to verify phone number");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePhoneNumber()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.SetPhoneNumberAsync(user, null);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { area = "Auth", Message = ManageMessageId.RemovePhoneSuccess });
                }
            }
            return RedirectToAction(nameof(Index), new { area = "Auth", Message = ManageMessageId.Error });
        }



        // GET: /Manage/SetPassword
        [HttpGet]
        public IActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { area = "Auth", Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { area = "Auth", Message = ManageMessageId.Error });
        }

        //GET: /Manage/ManageLogins
        [HttpGet]
        public async Task<IActionResult> ManageLogins(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.AddLoginSuccess ? "The external login was added."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await _userManager.GetLoginsAsync(user);
            //var otherLogins = await _signInManager.GetExternalAuthenticationSchemesAsync().Where(auth => userLogins.All(ul => auth.AuthenticationScheme != ul.LoginProvider)).ToList();
            ViewData["ShowRemoveButton"] = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                //OtherLogins = otherLogins
            });
        }

       

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            AddLoginSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion
    }
}
