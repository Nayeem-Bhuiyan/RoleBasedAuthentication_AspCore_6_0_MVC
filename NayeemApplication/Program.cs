using NayeemApplication.Data;
using NayeemApplication.Data.Entity.ApplicationUsersEntity;
using NayeemApplication.Repository.CrudRepository;
using NayeemApplication.Repository.CrudRepository.Interface;
using NayeemApplication.Services.AuthService;
using NayeemApplication.Services.AuthService.Interfaces;
using NayeemApplication.Services.MailService;
using NayeemApplication.Services.MailService.Interface;
using NayeemApplication.Settings;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using NayeemApplication.Services.CityService.Interface;
using NayeemApplication.Services.CityService;
using NayeemApplication.Services.CountryService.Interface;
using NayeemApplication.Services.CountryService;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseWebRoot(Path.Combine(Directory.GetCurrentDirectory(),"wwwroot"));
builder.Logging.AddJsonConsole();

#region ProjectSetUp


builder.Services.AddRazorPages();
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".AdventureWorks.Session";
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.IsEssential = true;
});

//builder.Services.AddMvc(options =>
//{
//   // options.EnableEndpointRouting = false;
//});
builder.Services.AddMvc().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); ;
builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddHttpContextAccessor();

#endregion

#region App Database Settings
builder.Services.AddDbContext<AppDbContext>(OptionsBuilderConfigurationExtensions => OptionsBuilderConfigurationExtensions.UseSqlServer(
    builder.Configuration.GetConnectionString("AppDbConnection")
    ));
builder.Services.AddMemoryCache();
//.AddDefaultTokenProviders();
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
#endregion

#region Auth Related Settings
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(8);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(8);

    options.LoginPath = "/Auth/Account/Login";
    options.AccessDeniedPath = "/Auth/Account/AccessDenied";
    options.SlidingExpiration = true;
});
#endregion

#region Areas Config
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.AreaViewLocationFormats.Clear();
    options.AreaViewLocationFormats.Add("/Areas/{2}/{0}" + RazorViewEngine.ViewExtension);
    options.AreaViewLocationFormats.Add("/areas/{2}/Views/{1}/{0}.cshtml");
    options.AreaViewLocationFormats.Add("/areas/{2}/Views/Shared/{0}.cshtml");
    options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
    // {0} - Action Name
    // {1} - Controller Name
    // {2} - Area Name
});

#endregion

#region generic_Crud_Repository
builder.Services.AddScoped(typeof(IRepositoryService<>), typeof(RepositoryService<>));


#endregion

#region UserService
builder.Services.AddScoped<IUserInfoes, UserInfoes>();
builder.Services.AddScoped<IUserServiceSP,UserServiceSP>();
#endregion


#region MasterData
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<ICityService, CityService>();
#endregion

#region MailService
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();
//builder.Services.AddSingleton<IConfiguration>(builder.Configuration.Configuration);
builder.Services.AddTransient<IGeneralMailService, GeneralMailService>();
builder.Services.AddSingleton<EmailService>();
builder.Services.AddTransient<IEmailSender, AuthMessageSender>();
builder.Services.AddTransient<ISmsSender, AuthMessageSender>();
#endregion


#region Others



builder.Services.AddMvc().AddRazorPagesOptions(options =>
{
    options.Conventions.AddAreaPageRoute("Identity", "/Auth/Account/Login", "");
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.Configure<FormOptions>(x =>
{
    x.MultipartBodyLengthLimit = 85899345920;
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions
        .AddPageApplicationModelConvention("/OrderArea/SalesOrderInfo/CreateOrder",
            model =>
            {
                model.Filters.Add(new RequestSizeLimitAttribute(85899345920));
            });
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseSession();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

ILogger logger = app.Logger;
IHostApplicationLifetime lifetime = app.Lifetime;
IWebHostEnvironment env = app.Environment;
app.MapRazorPages();
//app.UseMvc(routes =>
//{
//    routes.MapRoute(
//    name: "MyArea",
//    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

//    routes.MapRoute(
//       name: "default",
//       template: "{controller=Home}/{action=Index}/{id?}");

//});
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "MyArea",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
