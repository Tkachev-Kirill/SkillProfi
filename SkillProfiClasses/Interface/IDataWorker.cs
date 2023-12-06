using SkillProfiClasses.AccountData;
using SkillProfiClasses.Pages.BlogPage;
using SkillProfiClasses.Pages.ContactPage;
using SkillProfiClasses.Pages.MainPage;
using SkillProfiClasses.Pages.ProjectPage;
using SkillProfiClasses.Pages.ServicePage;
using SkillProfiClasses.RequestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillProfiClasses.Interface
{
    public interface IDataWorker
    {
        public Task<string> Autentefications(string login, string password);

        public Task<MainPage> GetMainPage();
        public Task<string> GetImage(string nameFile);
        public Task<List<BlogPage>> GetDataAllBlog();
        public Task<BlogPage> GetConcreteBlog(string id);
        public Task<ContactPage> GetContact();
        public Task<List<ProjectPage>> GetDataAllProject();
        public Task<ProjectPage> GetConcreteProject(string id);
        public Task<List<ServicePage>> GetDataAllService();
        public Task<ServicePage> GetConcreteService(string id);
        public Task<List<Request>> GetDataAllRequest();
        public Task<List<Request>> GetDataAllRequestInDate(DateTime dateStart, DateTime dateFinish);
        
        public Task<Account> GetConcreteAccount(string id);
        public Task<List<SocialNetwork>> GetDataSocialNetwork();
        public Task<SocialNetwork> GetConcreteSocialNetwork(string id);
        public Task CreateNewAccount(Account acc);
        public Task CreateNewRequest(Request request);
        public Task CreateProject(ProjectPage project);
        public Task CreateService(ServicePage service);
        public Task CreateNetwork(SocialNetwork network);
        public Task CreateBlog(BlogPage blog);
        public Task UpdateMainPage(MainPage mainPage);
        public Task UpdateBlog(BlogPage blog);
        public Task UpdateProject(ProjectPage project);
        public Task UpdateService(ServicePage service);
        public Task UpdateContact(ContactPage contact);
        public Task UpdateSocialNetwork(SocialNetwork socialNetwork);
        public Task UpdateStatusRequest(Request request);
        public Task DeleteProject(int id);
        public Task DeleteService(int id);
        public Task DeleteBlog(int id);
        public Task DeleteSocialNetwork(int id);
    }
}
