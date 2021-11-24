using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Notifier.Frontend.Pages
{
    [Authorize]
    public class MessageModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
