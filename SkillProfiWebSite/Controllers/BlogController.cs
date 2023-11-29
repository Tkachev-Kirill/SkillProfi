using Microsoft.AspNetCore.Mvc;
using SkillProfiWebSite.Models;

namespace SkillProfiWebSite.Controllers
{
    public class BlogController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Home()
        {
            var blogs = await DataManager.GetDataAllBlog();

            foreach (var blog in blogs)
            {
                await DataManager.GetImage(blog.PathToImage);
            }
            return View(blogs);
        }

        [HttpGet]
        public async Task<IActionResult> ConcreteData(int id)
        {
            var project = await DataManager.GetConcreteBlog(id.ToString());
            return View(project);
        }
    }
}
