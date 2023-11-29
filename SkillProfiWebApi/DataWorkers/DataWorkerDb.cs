using SkillProfiClasses.AccountData;
using SkillProfiClasses.Interface;
using SkillProfiClasses.Pages.BlogPage;
using SkillProfiClasses.Pages.ContactPage;
using SkillProfiClasses.Pages.MainPage;
using SkillProfiClasses.Pages.ProjectPage;
using SkillProfiClasses.Pages.ServicePage;
using SkillProfiClasses.RequestData;
using SkillProfiWebApi.WorkerWithDb.CRUD;
using SkillProfiWebApi.WorkerWithDb.Helper;

namespace SkillProfiWebApi.DataWorkers
{
    public class DataWorkerDb : IDataWorker
    {
        private string PathDb { get; set; }
        public DataWorkerDb(string pathDB)
        {
            PathDb = pathDB;
        }
        public async Task CreateBlog(BlogPage blog)
        {
            var creater = new Creater(PathDb);
            await creater.CreateBlog(blog);
        }

        public async Task CreateNewAccount(Account acc)
        {
            var creater = new Creater(PathDb);
            await creater.CreateNewAccount(acc);
        }

        public async Task CreateNewRequest(Request request)
        {
            var creater = new Creater(PathDb);
            await creater.CreateNewRequest(request);
        }

        public async Task CreateProject(ProjectPage project)
        {
            var creater = new Creater(PathDb);
            await creater.CreateProject(project);
        }

        public async Task CreateService(ServicePage service)
        {
            var creater = new Creater(PathDb);
            await creater.CreateService(service);
        }

        public async Task DeleteBlog(int id)
        {
            var deleter = new Deleter(PathDb);
            await deleter.DeleteBlog(id);
        }

        public async Task DeleteProject(int id)
        {
            var deleter = new Deleter(PathDb);
            await deleter.DeleteProject(id);
        }

        public async Task DeleteService(int id)
        {
            var deleter = new Deleter(PathDb);
            await deleter.DeleteService(id);
        }

        public async Task DeleteSocialNetwork(int id)
        {
            var deleter = new Deleter(PathDb);
            await deleter.DeleteSocialNetwork(id);
        }

        public async Task<BlogPage> GetConcreteBlog(string id)
        {
            if (!int.TryParse(id, out int result))
            {
                return null;
            }
            var reader = new Reader(PathDb);
            return await reader.GetConcreteBlog(result);
        }

        public async Task<ContactPage> GetContact()
        {
            var reader = new Reader(PathDb);
            return await reader.GetContact();
        }

        public async Task<ProjectPage> GetConcreteProject(string id)
        {
            if (!int.TryParse(id, out int result))
            {
                return null;
            }
            var reader = new Reader(PathDb);
            return await reader.GetConcreteProject(result);
        }

        public async Task<SocialNetwork> GetConcreteSocialNetwork(string id)
        {
            if (!int.TryParse(id, out int result))
            {
                return null;
            }
            var reader = new Reader(PathDb);
            return await reader.GetConcreteSocialNetwork(result);
        }

        public async Task<List<Request>> GetDataAllRequestInDate(DateTime dateStart, DateTime dateFinish)
        {
            var reader = new Reader(PathDb);
            return await reader.GetDataAllRequestInDate(dateStart, dateFinish);
        }

        public async Task<ServicePage> GetConcreteService(string id)
        {
            if (!int.TryParse(id, out int result))
            {
                return null;
            }
            var reader = new Reader(PathDb);
            return await reader.GetConcreteService(result);
        }

        public async Task<List<BlogPage>> GetDataAllBlog()
        {
            var reader = new Reader(PathDb);
            return await reader.GetDataAllBlog();
        }

        public async Task<MainPage> GetMainPage()
        {
            var reader = new Reader(PathDb);
            return await reader.GetmainPage();
        }


        public async Task<List<ProjectPage>> GetDataAllProject()
        {
            var reader = new Reader(PathDb);
            return await reader.GetDataAllProject();
        }

        public async Task<List<Request>> GetDataAllRequest()
        {
            var reader = new Reader(PathDb);
            return await reader.GetDataAllRequest();
        }

        public async Task<List<ServicePage>> GetDataAllService()
        {
            var reader = new Reader(PathDb);
            return await reader.GetDataAllService();
        }

        public async Task UpdateBlog(BlogPage blog)
        {
            var updater = new Updater(PathDb);
            await updater.UpdateBlog(blog);
        }

        public async Task UpdateContact(ContactPage contact)
        {
            var updater = new Updater(PathDb);
            await updater.UpdateContact(contact);
        }

        public async Task UpdateMainPage(MainPage mainPage)
        {
            var updater = new Updater(PathDb);
            await updater.UpdateMainPage(mainPage);
        }

        public async Task UpdateProject(ProjectPage project)
        {
            var updater = new Updater(PathDb);
            await updater.UpdateProject(project);
        }

        public async Task UpdateService(ServicePage service)
        {
            var updater = new Updater(PathDb);
            await updater.UpdateService(service);
        }

        public async Task UpdateSocialNetwork(SocialNetwork socialNetwork)
        {
            var updater = new Updater(PathDb);
            await updater.UpdateSocialNetwork(socialNetwork);
        }

        public async Task UpdateStatusRequest(Request request)
        {
            var updater = new Updater(PathDb);
            await updater.UpdateStatusRequest(request);
        }


        public async Task<bool> Autentefications(string login, string password)
        {
            var reader = new Reader(PathDb);
            var needData = await reader.GetConcreteAccount(login);
            if (needData is null)
            {
                return false;
            }
            if (needData.Password != password)
            {
                return false;
            }
            return true;
        }

        public async Task<List<Account>> GetDataAllAccount()
        {
            var reader = new Reader(PathDb);
            return await reader.GetAllAccount();
        }

        public async Task<Account> GetConcreteAccount(string login)
        {
            var reader = new Reader(PathDb);
            return await reader.GetConcreteAccount(login);
        }

        public async Task<List<SocialNetwork>> GetDataSocialNetwork()
        {
            var reader = new Reader(PathDb);
            return await reader.GetDataSocialNetwork();
        }

        public async Task CreateNetwork(SocialNetwork network)
        {
            var creater = new Creater(PathDb);
            await creater.CreateNetwork(network);
        }

        public Task<string> GetImage(string nameFile)
        {
            throw new NotImplementedException();
        }
    }
}
