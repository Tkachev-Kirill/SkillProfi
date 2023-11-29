using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SkillProfiClasses.Pages.MainPage;
using SkillProfiClasses.RequestData;
using SkillProfiWebSite.Models;
using System.Text;

namespace SkillProfiWebSite.Controllers
{
    public class RequestController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Home()
        {
            var mainPage = await DataManager.GetMainPage();

            if (mainPage is null)
            {
                mainPage = new MainPage()
                {
                    NameBaner = "Отсутуют данные в базе",
                    TextInBaner = "Отсутуют данные в базе",
                    TextUnderBaner = "Отсутуют данные в базе"
                };
            }

            return View(new MainPageViewModel()
            {
                Success = null,
                DataMainPage = mainPage,
                DataRequest = new Request {Email= string.Empty, Name = string.Empty, Text= string.Empty }
            });
        }

        [HttpPost]
        public async Task<IActionResult> Home(Request data)
        {
            data.Date = DateTime.Now;
            data.RequestTypeNum = 1;
            data.RequestStatusNum = 0;
            var success =await DataManager.CreateNewRequest(data);

            var mainPage = await DataManager.GetMainPage();

            return View(new MainPageViewModel()
            {
                Success = success,
                DataMainPage = mainPage,
                DataRequest = success ? new Request { Email = string.Empty, Name = string.Empty, Text = string.Empty }: data
            });
        }
    }
}
