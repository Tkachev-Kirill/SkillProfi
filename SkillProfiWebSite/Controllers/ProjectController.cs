using Microsoft.AspNetCore.Mvc;
using SkillProfiWebSite.Models;

namespace SkillProfiWebSite.Controllers
{
    public class ProjectController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Home()
        {
            var projects = await DataManager.GetDataAllProject();
            foreach (var project in projects)
            {
                await DataManager.GetImage(project.PathToImage);
            }
            return View(projects);
        }

        [HttpGet]
        public async Task<IActionResult> ConcreteData(int id)
        {
            var project = await DataManager.GetConcreteProject(id.ToString());
            return View(project);
        }
    }
}
