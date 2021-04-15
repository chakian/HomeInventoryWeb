using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebUI.Base;

namespace WebUI.Pages.Home
{
    public class CreateModel : BasePageModel<CreateModel>
    {
        public CreateModel(ILogger<CreateModel> logger, IHomeService homeService) : base(logger, homeService)
        {
        }

        [BindProperty]
        public HomeEntity Home { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            homeService.CreateHome(Home, UserId);

            return RedirectToPage("/Index");
        }
    }
}
