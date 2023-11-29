using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SkillProfiClasses.AccountData;
using SkillProfiClasses.Pages.BlogPage;
using SkillProfiClasses.Pages.ContactPage;
using SkillProfiClasses.Pages.MainPage;
using SkillProfiClasses.Pages.ProjectPage;
using SkillProfiClasses.Pages.ServicePage;
using SkillProfiClasses.RequestData;
using SkillProfiWebApi.WorkerWithDb.Context;
using System;

namespace SkillProfiWebApi.WorkerWithDb.CRUD
{
    public class Reader
    {
        private ModelContext Context { get; set; }
        public Reader(string strCon)
        {
            DbContextOptionsBuilder<ModelContext> optionsBuilder = new DbContextOptionsBuilder<ModelContext>();
            Context = new ModelContext(optionsBuilder.UseSqlServer(strCon).Options);
        }
        public async Task<List<BlogPage>> GetDataAllBlog()
        {
            return await Context.BlogPages.ToListAsync();
        }
        public async Task<BlogPage> GetConcreteBlog(int id)
        {
            return await Context.BlogPages.FindAsync(id);
        }
        public async Task<ContactPage> GetContact()
        {
            return await Context.ContactPages.FirstOrDefaultAsync();
        }
        public async Task<List<ProjectPage>> GetDataAllProject()
        {
            return await Context.ProjectPages.ToListAsync();
        }
        public async Task<List<SocialNetwork>> GetDataSocialNetwork()
        {
            return await Context.SocialNetworks.ToListAsync();
        }
        public async Task<ProjectPage> GetConcreteProject(int id)
        {
            return await Context.ProjectPages.FindAsync(id);
        }
        public async Task<List<ServicePage>> GetDataAllService()
        {
            return await Context.ServicePages.ToListAsync();
        }
        public async Task<ServicePage> GetConcreteService(int id)
        {
            return await Context.ServicePages.FindAsync(id);
        }
        public async Task<List<Request>> GetDataAllRequest()
        {
            return await Context.Requests.ToListAsync();
        }
        public async Task<MainPage> GetmainPage()
        {
            return await Context.MainPages.FirstOrDefaultAsync();
        }
        public async Task<List<Request>> GetDataAllRequestInDate(DateTime start, DateTime finish)
        {
            if (finish == default(DateTime))
            {
                return await Context.Requests.Where(x => x.Date.Day == start.Day).ToListAsync();
            }
            return await Context.Requests.Where(x => x.Date >= start && x.Date <= finish).ToListAsync();
        }
        public async Task<SocialNetwork> GetConcreteSocialNetwork(int id)
        {
            return await Context.SocialNetworks.FindAsync(id);
        }
        public async Task<List<Account>> GetAllAccount()
        {
            return await Context.Accounts.ToListAsync();
        }
        public async Task<Account> GetConcreteAccount(string login)
        {
            return await Context.Accounts.Where(x=>x.Login == login).FirstOrDefaultAsync();
        }
    }
}
