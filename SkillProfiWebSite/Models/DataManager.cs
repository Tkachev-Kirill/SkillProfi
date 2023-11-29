using Newtonsoft.Json;
using SkillProfiClasses.Pages.BlogPage;
using SkillProfiClasses.Pages.ContactPage;
using SkillProfiClasses.Pages.MainPage;
using SkillProfiClasses.Pages.ProjectPage;
using SkillProfiClasses.Pages.ServicePage;
using SkillProfiClasses.RequestData;
using System.Text;

namespace SkillProfiWebSite.Models
{
    public static class DataManager
    {
        public static async Task<MainPage> GetMainPage()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:7276/api/GetMainPage");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<MainPage>(jsonString);
                    return data;
                }
                else
                {
                    return null;
                }
            }
        }

        public static async Task<bool> CreateNewRequest(Request request)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var resp = await client.PostAsync("https://localhost:7276/api/CreateNewRequest", content);
                return resp.IsSuccessStatusCode;
            }
        }

        public static async Task<List<ServicePage>> GetDataAllService()
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

        public static async Task<List<ProjectPage>> GetDataAllProject()
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

        public static async Task<string> GetNeedPath()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:7276/api/GetNeedPath");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static async Task<string> GetImage(string nameFile)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"https://localhost:7276/api/GetImage/{nameFile}");
                var stream = await response.Content.ReadAsStreamAsync();
                var fileName = nameFile;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName);
                using (var fileStream = File.Create(filePath))
                {
                    await stream.CopyToAsync(fileStream);
                }
                return fileName;
            }
        }

        public static async Task<ProjectPage> GetConcreteProject(string id)
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
        public static async Task<List<BlogPage>> GetDataAllBlog()
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

        public static async Task<BlogPage> GetConcreteBlog(string id)
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

        public static async Task<ContactPage> GetContact()
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

        public static async Task<List<SocialNetwork>> GetDataSocialNetwork()
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
    }
}
