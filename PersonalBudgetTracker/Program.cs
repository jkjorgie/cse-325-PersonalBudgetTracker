using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalBudgetTracker.Components;
using PersonalBudgetTracker.Data;
using PersonalBudgetTracker.Models;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.SignIn.RequireConfirmedAccount = false;
}).AddEntityFrameworkStores<ApplicationDbContext>()
  .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login";
});

// Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Auth endpoints — must be real HTTP request/response to set/clear cookies
app.MapPost("/account/logout", async (SignInManager<ApplicationUser> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Redirect("/");
}).DisableAntiforgery();

app.MapPost("/account/login-post", async (
    HttpContext ctx,
    SignInManager<ApplicationUser> signInManager,
    [Microsoft.AspNetCore.Mvc.FromForm] string email,
    [Microsoft.AspNetCore.Mvc.FromForm] string password) =>
{
    var result = await signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);
    if (result.Succeeded)
        return Results.Redirect("/");
    return Results.Redirect("/account/login?error=invalid");
}).DisableAntiforgery();

app.MapPost("/account/register-post", async (
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    [Microsoft.AspNetCore.Mvc.FromForm] string email,
    [Microsoft.AspNetCore.Mvc.FromForm] string password) =>
{
    var user = new ApplicationUser { UserName = email, Email = email };
    var result = await userManager.CreateAsync(user, password);
    if (result.Succeeded)
    {
        await signInManager.SignInAsync(user, isPersistent: false);
        return Results.Redirect("/");
    }
    var error = Uri.EscapeDataString(string.Join(" ", result.Errors.Select(e => e.Description)));
    return Results.Redirect($"/account/register?error={error}");
}).DisableAntiforgery();

app.Run();
