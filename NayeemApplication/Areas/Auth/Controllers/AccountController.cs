using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NayeemApplication.Services.AuthService.Interfaces;
using NayeemApplication.Helpers;
using NayeemApplication.Data.Entity.ApplicationUsersEntity;
using System.Net;
using NayeemApplication.Services.MailService.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using NayeemApplication.Areas.Auth.Models.AccountViewModels;
using NayeemApplication.Repository.CrudRepository.Interface;
using NayeemApplication.Data.Entity.MasterDataEntity;
using NayeemApplication.Services.CountryService.Interface;
using NayeemApplication.Services.CityService.Interface;

namespace NayeemApplication.Areas.Auth.Controllers
{
    [Authorize]
    [Area("Auth")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserInfoes userInfoes;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IPasswordValidator<ApplicationUser> _passwordValidator;
        private readonly IUserValidator<ApplicationUser> _userValidator;
        private readonly ILogger<RegisterViewModel> _logger;
        private readonly IMailService _mailService;
        private readonly IGeneralMailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IWebHostEnvironment _environment;
        public readonly ICountryService _countryService;
        public readonly ICityService _cityService;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IUserInfoes userInfoes,
            IPasswordHasher<ApplicationUser> passwordHash,
            IPasswordValidator<ApplicationUser> passwordVal,
            IUserValidator<ApplicationUser> userValid,
            ILogger<RegisterViewModel> logger,
            IMailService mailService,
            IGeneralMailService emailService,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment environment,
            ICountryService countryService,
            ICityService cityService
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            this.userInfoes = userInfoes;
            _passwordHasher = passwordHash;
            _passwordValidator = passwordVal;
            _userValidator = userValid;
            _logger = logger;
            _mailService = mailService;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
            _countryService = countryService;
            _cityService = cityService;
        }


        #region Register
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            ViewBag.Countries = await _countryService.GetAllCountrysAsync();

            return View();
        }




        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewBag.Countries = await _countryService.GetAllCountrysAsync();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    TwoFactorEnabled = false,
                    PhoneNumber = model.PhoneNumber,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    ApplicationUser CurrentUser = await userInfoes.GetUserInfoByEmailAsync(model.Email);

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    //bool response = await _mailService.SendTextEmailAsync(model.Email, "Confirm your account", $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>Click Here</a>");
                    bool response = await _mailService.SendTextEmailAsync(model.Email, "Confirm your account", $"Please confirm your account by clicking this link: <a style='display: inline-block;background-color: #008000;color: #FFFFFF;padding: 14px 25px;text-align: center;text-decoration: none;font-size: 16px;margin-left: 20px;opacity: 0.9' href='{callbackUrl}'>Click Here</a>");
                    if (response)
                    {
                        _logger.LogInformation(1, "User created a new account with password.");
                        return RedirectToAction(nameof(EmailSuccess), new { area = "Auth", ReturnUrl = returnUrl });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Email to Send");
                        return View(model);
                    }
                }
                AddErrors(result);
            }
            return View(model);
        }

        #endregion

        #region Login
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LogInViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                ApplicationUser userInfo = new ApplicationUser();
                if (model != null)
                {
                    userInfo = await userInfoes.GetUserInfoByUser(model.Name);
                    if (userInfo == null)
                    {
                        userInfo = await userInfoes.GetUserInfoByEmailAsync(model.Name);
                        model.Name = userInfo?.UserName;
                    }
                }


                if (userInfo != null)
                {
                    if (userInfo.isActive == true)
                    {
                        var result = await _signInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, lockoutOnFailure: false);
                        //return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });


                        if (result.Succeeded)
                        {
                            _logger.LogInformation(1, "User logged in.");
                            var ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                            var userAgent = Request.Headers["User-Agent"].ToString();
                            var mechineName = Environment.MachineName;
                            var rip = Dns.GetHostEntry(HttpContext.Connection.RemoteIpAddress.ToString()).ToString();

                            var userRole = await userInfoes.GetUserRoleByUserName(model.Name);

                            if (userRole == "Super Admin")
                            {
                                return RedirectToLocal(returnUrl);
                            }
                            if (userRole == "Admin")
                            {
                                return RedirectToLocal(returnUrl);
                            }
                            if (userRole == "Manager")
                            {
                                return RedirectToLocal(returnUrl);
                            }
                            if (userRole == "General User")
                            {
                                return RedirectToLocal(returnUrl);
                            }
                            else
                            {
                                return RedirectToLocal(returnUrl);
                            }

                        }
                        if (result.RequiresTwoFactor)
                        {
                            return RedirectToAction(nameof(SendCode), new { returnUrl, model.RememberMe });
                        }
                        if (result.IsLockedOut)
                        {
                            _logger.LogWarning("User account locked out.");
                            return RedirectToAction(nameof(Lockout));
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
            return View(model);
        }







        #endregion

        #region Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {

            var ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var userAgent = Request.Headers["User-Agent"].ToString();
            var mechineName = Environment.MachineName;
            var rip = Dns.GetHostEntry(HttpContext.Connection.RemoteIpAddress.ToString()).ToString();
           var userInfo = await userInfoes.GetUserInfoByUser(User.Identity.Name);
            await _signInManager.SignOutAsync();
            return Redirect("/Home/Index");

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }
        #endregion

        #region Email Confirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult EmailSuccess()
        {
            return View();
        }

        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
        #endregion

        #region Forgot Password

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action(nameof(ResetPassword), "Account", new {area="Auth", userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                await _mailService.SendTextEmailAsync(model.Email, "Reset Password",
                   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }



        // GET: /Auth/Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        
        // POST: /Auth/Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account",new {area="Auth"});
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account", new { area = "Auth" });
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Auth/Account/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        #endregion

        #region TwoStep Verification
        // GET: /Auth/Account/SendCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
        {
           
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }

            // Generate the token and send it
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, model.SelectedProvider);
            if (string.IsNullOrWhiteSpace(code))
            {
                return View("Error");
            }

            var message = "Your security code is: " + code;
            if (model.SelectedProvider == "Email")
            {
                await _mailService.SendTextEmailAsync(await _userManager.GetEmailAsync(user), "Security Code", message);
            }
            else if (model.SelectedProvider == "Phone")
            {
                await _mailService.SendTextEmailAsync(await _userManager.GetPhoneNumberAsync(user), "SMS Auth", message);
            }

            return RedirectToAction(nameof(VerifyCode), new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

      


        //
        // GET: /Auth/Account/VerifyCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode(string provider, bool rememberMe, string returnUrl = null)
        {
            // Require that the user has already logged in via username/password or external login
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Auth/Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes.
            // If a user enters incorrect codes for a specified amount of time then the user account
            // will be locked out for a specified amount of time.
            var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
            if (result.Succeeded)
            {
                return RedirectToLocal(model.ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning(7, "User account locked out.");
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid code.");
                return View(model);
            }
        }
        #endregion

        #region ChangePassword

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePsswordViewModel model)
        {
            try
            {
                IdentityResult response;
                response= await _userManager.ChangePasswordAsync(await _userManager.FindByNameAsync(HttpContext.User.Identity.Name), model.OldPassword, model.Password);
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

        #endregion

        #region Check UserInfo
        [AllowAnonymous]
        [HttpGet]
        public async Task<string> RestrictDuplicateUserName(string uName)
        {
            return await userInfoes.CheckUserName(uName);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<string> RestrictDuplicateEmail(string email)
        {
            return await userInfoes.CheckEmail(email);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<string> RestrictDuplicatePhone(string MobileNum)
        {
            return await userInfoes.CheckPhone(MobileNum);
        }

        #endregion

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                //return Redirect(returnUrl);
                var userId = HttpContext.User.Identity.Name;

                //return RedirectToAction(nameof(HomeController.Index), "Home", new { area = "" });
                return Redirect("~/Home/Index");
            }
            else
            {
                var userId = HttpContext.User.Identity.Name;

                //return RedirectToAction(nameof(HomeController.Index), "Home", new { area = "" });
                return Redirect("~/Home/Index");
            }
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion

        #region AccessDenied
        //
        // GET:/Auth/Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion

    }

}