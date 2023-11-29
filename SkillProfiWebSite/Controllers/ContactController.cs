using Microsoft.AspNetCore.Mvc;
using SkillProfiClasses.Pages.ContactPage;
using SkillProfiClasses.Pages.MainPage;
using SkillProfiClasses.RequestData;
using SkillProfiWebSite.Models;

namespace SkillProfiWebSite.Controllers
{
    public class ContactController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Home()
        {
            var contact = await DataManager.GetContact();

            if (contact is null)
            {
                contact = new ContactPage()
                {
                    Email = "Отсутуют данные в базе",
                    Address = "Отсутуют данные в базе",
                    Phone = "Отсутуют данные в базе"
                };
            }

            var networks = await DataManager.GetDataSocialNetwork();
            foreach (var network in networks)
            {
                 await DataManager.GetImage(network.PathToImage);
            }

            return View(new ContactPageViewModel()
            {
                Contact = contact,
                SocialNetwork = networks
            });
        }
    }
}
