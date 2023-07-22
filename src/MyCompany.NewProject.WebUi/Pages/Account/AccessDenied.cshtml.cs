using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyCompany.NewProject.WebUi.Pages.Account;

[AllowAnonymous]
public class AccessDeniedModel : PageModel
{
    [FromQuery]
    public required string ReturnUrl { get; set; }
}