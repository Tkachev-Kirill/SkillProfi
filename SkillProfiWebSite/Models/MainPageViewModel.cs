using SkillProfiClasses.Pages.MainPage;
using SkillProfiClasses.RequestData;

namespace SkillProfiWebSite.Models
{
    public class MainPageViewModel
    {
        public bool? Success { get; set; }
        public MainPage DataMainPage { get; set; }
        public Request DataRequest { get; set; }
    }
}
