using Newtonsoft.Json;
using SkillProfiClasses.AccountData;
using SkillProfiClasses.Interface;
using SkillProfiClasses.Pages.BlogPage;
using SkillProfiClasses.Pages.ContactPage;
using SkillProfiClasses.Pages.MainPage;
using SkillProfiClasses.Pages.ProjectPage;
using SkillProfiClasses.Pages.ServicePage;
using SkillProfiClasses.RequestData;
using SkillProfiDesktopAdmin.Page;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace SkillProfiDesktopAdmin.Workers
{
    internal class DesktopWorker : IDataWorker
    {
        public async Task<bool> Autentefications(string login, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"https://localhost:7276/api/Autentefications/{login}/{password}");
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task CreateBlog(BlogPage blog)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(blog);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
               var res = await client.PostAsync("https://localhost:7276/api/CreateBlog", content);
            }
        }

        public async Task CreateNewAccount(Account acc)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(acc);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync("https://localhost:7276/api/CreateNewAccount", content);
            }
        }

        public async Task CreateNewRequest(Request request)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync("https://localhost:7276/api/CreateNewRequest", content);
            }
        }

        public async Task CreateProject(ProjectPage project)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(project);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync("https://localhost:7276/api/CreateProject", content);
            }
        }

        public async Task CreateService(ServicePage service)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(service);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync("https://localhost:7276/api/CreateService", content);
            }
        }

        public async Task DeleteBlog(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.DeleteAsync($"https://localhost:7276/api/DeleteBlog/{id}");
            }
        }

        public async Task DeleteProject(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.DeleteAsync($"https://localhost:7276/api/DeleteProject/{id}");
            }
        }

        public async Task DeleteService(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.DeleteAsync($"https://localhost:7276/api/DeleteService/{id}");
            }
        }

        public async Task DeleteSocialNetwork(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.DeleteAsync($"https://localhost:7276/api/DeleteSocialNetwork/{id}");
            }
        }

        public async Task<Account> GetConcreteAccount(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"https://localhost:7276/api/GetConcreteAccount/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Account>(jsonString);
                }
                else
                {
                    return new Account();
                }
            }
        }

        public async Task<BlogPage> GetConcreteBlog(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"https://localhost:7276/api/GetConcreteBlog/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<BlogPage>(jsonString);
                }
                else
                {
                    return new BlogPage();
                }
            }
        }

        public async Task<ContactPage> GetContact()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"https://localhost:7276/api/GetContact");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ContactPage>(jsonString);
                }
                else
                {
                    return new ContactPage();
                }
            }
        }

        public async Task<ProjectPage> GetConcreteProject(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"https://localhost:7276/api/GetConcreteProject/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ProjectPage>(jsonString);
                }
                else
                {
                    return new ProjectPage();
                }
            }
        }

        public async Task<List<Request>> GetDataAllRequestInDate(DateTime dateStart, DateTime dateFinish)
        {
            List<Request> data = new List<Request>();
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"https://localhost:7276/api/GetDataAllRequestInDate/{dateStart.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK")}/{dateFinish.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK")}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Request>>(jsonString);
                }
                else
                {
                    return data;
                }
            }
        }


        public async Task<ServicePage> GetConcreteService(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"https://localhost:7276/api/GetConcreteService/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ServicePage>(jsonString);
                }
                else
                {
                    return new ServicePage();
                }
            }
        }

        public async Task<SocialNetwork> GetConcreteSocialNetwork(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"https://localhost:7276/api/GetConcreteSocialNetwork/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<SocialNetwork>(jsonString);
                }
                else
                {
                    return new SocialNetwork();
                }
            }
        }

        public async Task<List<Account>> GetDataAllAccount()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:7276/api/GetDataAllAccount");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Account>>(jsonString);
                }
                else
                {
                    return new List<Account>();
                }
            }
        }

      

        public async Task<List<BlogPage>> GetDataAllBlog()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:7276/api/GetDataAllBlog");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<BlogPage>>(jsonString);
                }
                else
                {
                    return new List<BlogPage>();
                }
            }
        }

        public async Task<MainPage> GetMainPage()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:7276/api/GetMainPage");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MainPage>(jsonString);
                }
                else
                {
                    return new MainPage();
                }
            }
        }

        public async Task<List<ProjectPage>> GetDataAllProject()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:7276/api/GetDataAllProject");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<ProjectPage>>(jsonString);
                }
                else
                {
                    return new List<ProjectPage>();
                }
            }
        }

        public async Task<List<Request>> GetDataAllRequest()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:7276/api/GetDataAllRequest");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Request>>(jsonString);
                }
                else
                {
                    return new List<Request>();
                }
            }
        }

        public async Task<List<ServicePage>> GetDataAllService()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:7276/api/GetDataAllService");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<ServicePage>>(jsonString);
                }
                else
                {
                    return new List<ServicePage>();
                }
            }
        }

        public async Task UpdateBlog(BlogPage blog)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(blog);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync("https://localhost:7276/api/UpdateBlog", content);
            }
        }

        public async Task UpdateContact(ContactPage contact)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(contact);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync("https://localhost:7276/api/UpdateContact", content);
            }
        }

        public async Task UpdateMainPage(MainPage mainPage)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(mainPage);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync("https://localhost:7276/api/UpdateMainPage", content);
            }
        }

        public async Task UpdateProject(ProjectPage project)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(project);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync("https://localhost:7276/api/UpdateProject", content);
            }
        }

        public async Task UpdateService(ServicePage service)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(service);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync("https://localhost:7276/api/UpdateService", content);
            }
        }

        public async Task UpdateSocialNetwork(SocialNetwork socialNetwork)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(socialNetwork);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync("https://localhost:7276/api/UpdateSocialNetwork", content);
            }
        }

        public async Task UpdateStatusRequest(Request request)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync("https://localhost:7276/api/UpdateStatusRequest", content);
            }
        }

        public async Task<List<SocialNetwork>> GetDataSocialNetwork()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:7276/api/GetDataSocialNetwork");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<SocialNetwork>>(jsonString);
                }
                else
                {
                    return new List<SocialNetwork>();
                }
            }
        }

        public async Task CreateNetwork(SocialNetwork network)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(network);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var res = await client.PostAsync("https://localhost:7276/api/CreateNetwork", content);
            }
        }

        public async Task <string> GetImage(string nameFile)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"https://localhost:7276/api/GetImage/{nameFile}");
                var stream = await response.Content.ReadAsStreamAsync();
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,Guid.NewGuid().ToString()+".jpeg");
                using (var fileStream = File.Create(filePath))
                {
                    await stream.CopyToAsync(fileStream);
                }
                return filePath;
            }
        }

        public async Task <string> UploadFile(string nameFile)
        {
            using (HttpClient client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(nameFile));
                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                    {
                        Name = "file",
                        FileName = nameFile
                    };
                    content.Add(fileContent);
                    var response = await client.PostAsync("https://localhost:7276/api/UploadImage", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        return jsonString;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }

        }
    }
}
