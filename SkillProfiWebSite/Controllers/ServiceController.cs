using Microsoft.AspNetCore.Mvc;
using SkillProfiClasses.Pages.MainPage;
using SkillProfiClasses.RequestData;
using SkillProfiWebSite.Models;

namespace SkillProfiWebSite.Controllers
{
    public class ServiceController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Home()
        {
            var services = await DataManager.GetDataAllService();
            return View(services);
        }
    }
}
