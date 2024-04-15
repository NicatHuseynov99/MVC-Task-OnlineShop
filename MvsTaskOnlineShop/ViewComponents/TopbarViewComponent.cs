using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvsTaskOnlineShop.Models;

namespace MvsTaskOnlineShop.ViewComponents
{
    public class TopbarViewComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        public TopbarViewComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                return await Task.FromResult(View(user));
            }
            else
            {
                AppUser user = null;
                return await Task.FromResult(View(user));
            }
        }
    }
}
