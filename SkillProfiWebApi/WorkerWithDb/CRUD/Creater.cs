using Microsoft.EntityFrameworkCore;
using SkillProfiClasses.AccountData;
using SkillProfiClasses.Pages.BlogPage;
using SkillProfiClasses.Pages.ContactPage;
using SkillProfiClasses.Pages.ProjectPage;
using SkillProfiClasses.Pages.ServicePage;
using SkillProfiClasses.RequestData;
using SkillProfiWebApi.WorkerWithDb.Context;

namespace SkillProfiWebApi.WorkerWithDb.CRUD
{
    public class Creater
    {
        private ModelContext Context { get; set; }
        public Creater(string strCon)
        {
            DbContextOptionsBuilder<ModelContext> optionsBuilder = new DbContextOptionsBuilder<ModelContext>();
            Context = new ModelContext(optionsBuilder.UseSqlServer(strCon).Options);
        }
        public async Task CreateNewAccount(Account acc)
        {
            await Context.Accounts.AddAsync(acc);
            await Context.SaveChangesAsync();
        }

        public async Task CreateNetwork(SocialNetwork network)
        {
            await Context.SocialNetworks.AddAsync(network);
            await Context.SaveChangesAsync();
        }
        
        public async Task CreateNewRequest(Request request)
        {
            await Context.Requests.AddAsync(request);
            await Context.SaveChangesAsync();
        }
        public async Task CreateProject(ProjectPage project)
        {
            await Context.ProjectPages.AddAsync(project);
            await Context.SaveChangesAsync();
        }
        public async Task CreateService(ServicePage service)
        {
            await Context.ServicePages.AddAsync(service);
            await Context.SaveChangesAsync();
        }
        public async Task CreateBlog(BlogPage blog)
        {
            await Context.BlogPages.AddAsync(blog);
            await Context.SaveChangesAsync();
        }
        public async Task CreateSocialNetwork(SocialNetwork socialNetwork)
        {
            await Context.SocialNetworks.AddAsync(socialNetwork);
            await Context.SaveChangesAsync();
        }
    }
}
