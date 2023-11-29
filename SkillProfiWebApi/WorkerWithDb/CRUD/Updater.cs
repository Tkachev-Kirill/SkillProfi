using Microsoft.EntityFrameworkCore;
using SkillProfiClasses.Pages.BlogPage;
using SkillProfiClasses.Pages.ContactPage;
using SkillProfiClasses.Pages.MainPage;
using SkillProfiClasses.Pages.ProjectPage;
using SkillProfiClasses.Pages.ServicePage;
using SkillProfiClasses.RequestData;
using SkillProfiWebApi.WorkerWithDb.Context;

namespace SkillProfiWebApi.WorkerWithDb.CRUD
{
    public class Updater
    {
        private ModelContext Context { get; set; }
        public Updater(string strCon)
        {
            DbContextOptionsBuilder<ModelContext> optionsBuilder = new DbContextOptionsBuilder<ModelContext>();
            Context = new ModelContext(optionsBuilder.UseSqlServer(strCon).Options);
        }
        public async Task UpdateMainPage(MainPage mainPage)
        {
            var data = await Context.MainPages.FirstOrDefaultAsync();
            if (data is null)
            {
                return;
            }
            data.NameBaner = mainPage.NameBaner;
            data.TextInBaner = mainPage.TextInBaner;
            data.TextUnderBaner = mainPage.TextUnderBaner;
            Context.Update(data);
            await Context.SaveChangesAsync();
        }
        public async Task UpdateBlog(BlogPage blog)
        {
            var data = await Context.BlogPages.FindAsync(blog.BlogId);
            if (data is null)
            {
                return;
            }
            data.Name = blog.Name;
            data.Description = blog.Description;
            data.Text = blog.Text;
            data.PathToImage = blog.PathToImage;
            Context.Update(data);
            await Context.SaveChangesAsync();
        }
        public async Task UpdateProject(ProjectPage project)
        {
            var data = await Context.ProjectPages.FindAsync(project.ProjectId);
            if (data is null)
            {
                return;
            }
            data.Name = project.Name;
            data.Description = project.Description;
            data.PathToImage = project.PathToImage;
            Context.Update(data);
            await Context.SaveChangesAsync();
        }
        public async Task UpdateService(ServicePage service)
        {
            var data = await Context.ServicePages.FindAsync(service.ServiceId);
            if (data is null)
            {
                return;
            }
            data.Name = service.Name;
            data.Description = service.Description;
            Context.Update(data);
            await Context.SaveChangesAsync();
        }
        public async Task UpdateContact(ContactPage contact)
        {
            var data = await Context.ContactPages.FirstOrDefaultAsync();
            if (data is null)
            {
                return;
            }
            data.Phone = contact.Phone;
            data.Email = contact.Email;
            data.Address = contact.Address;
            Context.Update(data);
            await Context.SaveChangesAsync();
        }
        public async Task UpdateSocialNetwork(SocialNetwork socialNetwork)
        {
            var data = await Context.SocialNetworks.FindAsync(socialNetwork.SocialNetworkId);
            if (data is null)
            {
                return;
            }
            data.PathToSite = socialNetwork.PathToSite;
            data.PathToImage = socialNetwork.PathToImage;
            Context.Update(data);
            await Context.SaveChangesAsync();
        }
        public async Task UpdateStatusRequest(Request request)
        {
            var data = await Context.Requests.FindAsync(request.RequestId);
            if (data is null)
            {
                return;
            }
            data.RequestStatusNum = request.RequestStatusNum;
            Context.Update(data);
            await Context.SaveChangesAsync();
        }
    }
}
