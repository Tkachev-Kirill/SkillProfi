using Microsoft.EntityFrameworkCore;
using SkillProfiClasses.AccountData;
using SkillProfiClasses.Interface;
using SkillProfiClasses.Pages.BlogPage;
using SkillProfiClasses.Pages.ContactPage;
using SkillProfiClasses.Pages.MainPage;
using SkillProfiClasses.Pages.ProjectPage;
using SkillProfiClasses.Pages.ServicePage;
using SkillProfiClasses.RequestData;
using SkillProfiWebApi.Helpers;
using SkillProfiWebApi.WorkerWithDb.Context;

namespace SkillProfiWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ModelContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));


            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            IDataWorker dataWorker = GetDataFromSettingsHelper.GetDataFromSettings(builder.Configuration);

            app.MapGet("/api/GetImage/{nameFile}", async (string nameFile, IConfiguration configuration) =>
            {
                var path = configuration.GetSection("NameFolder").Value;
                string needFolder = Path.Combine(@"C:\", path);
                string filePath = Path.Combine(needFolder, nameFile);
                return Results.File(filePath, "image/jpeg");
            });

            app.MapPost("/api/UploadImage", async (HttpRequest request, IConfiguration configuration) =>
            {
                var form = await request.ReadFormAsync();
                var file = form.Files["file"];

                if (file.Length > 0)
                {
                    string newName = Guid.NewGuid().ToString()+".jpeg";
                    var path = configuration.GetSection("NameFolder").Value;
                    string needFolder = Path.Combine(@"C:\", path);
                    string filePath = needFolder + "\\" + newName;
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return Results.Ok(newName);
                }

                return Results.BadRequest();
            });

            app.MapGet("/api/Autentefications/{login}/{password}", async (string login, string password) =>
            {
                var success = await dataWorker.Autentefications(login, password);
                if (success) return Results.Ok();
                return Results.NotFound(new { message = "Account not found" });
            });

            app.MapGet("/api/GetMainPage", () =>
            {
                return dataWorker.GetMainPage();
            });

            app.MapGet("/api/GetDataAllAccount", () =>
            {
                return dataWorker.GetDataAllAccount();
            });

            app.MapGet("/api/GetConcreteAccount/{id}", async (string id) =>
            {
                var needData = await dataWorker.GetConcreteAccount(id);
                if (needData is null) return Results.NotFound(new { message = "Account not found" });
                return Results.Json(needData);
            });

            app.MapGet("/api/GetDataAllBlog", () =>
            {
                return dataWorker.GetDataAllBlog();
            });
            app.MapGet("/api/GetConcreteBlog/{id}", async (string id) =>
            {
                var needData = await dataWorker.GetConcreteBlog(id);
                if (needData is null) return Results.NotFound(new { message = "Blog not found" });
                return Results.Json(needData);
            });

            app.MapGet("/api/GetContact", () =>
            {
                return dataWorker.GetContact();
            });
            app.MapGet("/api/GetDataSocialNetwork", () =>
            {
                return dataWorker.GetDataSocialNetwork();
            });
            app.MapGet("/api/GetDataAllProject", () =>
            {
                return dataWorker.GetDataAllProject();
            });
            app.MapGet("/api/GetConcreteProject/{id}", async (string id) =>
            {
                var needData = await dataWorker.GetConcreteProject(id);
                if (needData is null) return Results.NotFound(new { message = "Project not found" });
                return Results.Json(needData);
            });

            app.MapGet("/api/GetDataAllService", () =>
            {
                return dataWorker.GetDataAllService();
            });
            app.MapGet("/api/GetConcreteService/{id}", async (string id) =>
            {
                var needData = await dataWorker.GetConcreteService(id);
                if (needData is null) return Results.NotFound(new { message = "Service not found" });
                return Results.Json(needData);
            });

            app.MapGet("/api/GetConcreteSocialNetwork/{id}", async (string id) =>
            {
                var needData = await dataWorker.GetConcreteSocialNetwork(id);
                if (needData is null) return Results.NotFound(new { message = "network not found" });
                return Results.Json(needData);
            });


            app.MapGet("/api/GetDataAllRequest", () =>
            {
                return dataWorker.GetDataAllRequest();
            });
            app.MapGet("/api/GetDataAllRequestInDate/{dateStart}/{dateFinish}", async (DateTime dateStart, DateTime dateFinish) =>
            {
                if (dateStart == default(DateTime))
                {
                    return Results.BadRequest(new { message = "Invalid date format" });
                }
                var needData = await dataWorker.GetDataAllRequestInDate(dateStart, dateFinish);
                if (needData is null) return Results.NotFound(new { message = "Request not found" });
                return Results.Json(needData);
            });

            app.MapPost("/api/CreateNetwork", async (SocialNetwork network) =>
            {
                await dataWorker.CreateNetwork(network);
                return Results.Ok();
            });

            app.MapPost("/api/CreateNewAccount", async (Account acc) =>
            {
                await dataWorker.CreateNewAccount(acc);
                return Results.Ok();
            });

            app.MapPost("/api/CreateNewRequest", async (Request request) =>
            {
                await dataWorker.CreateNewRequest(request);
                Results.Ok();
            });

            app.MapPost("/api/CreateProject", async (ProjectPage project) =>
            {
                await dataWorker.CreateProject(project);
                Results.Ok();
            });

            app.MapPost("/api/CreateService", async (ServicePage service) =>
            {
                await dataWorker.CreateService(service);
                Results.Ok();
            });

            app.MapPost("/api/CreateBlog", async (BlogPage blog) =>
            {
                await dataWorker.CreateBlog(blog);
                Results.Ok();
            });

            app.MapPut("/api/UpdateMainPage", async (MainPage mainPage) =>
            {
                await dataWorker.UpdateMainPage(mainPage);
                Results.Ok();
            });

            app.MapPut("/api/UpdateBlog", async (BlogPage blog) =>
            {
                await dataWorker.UpdateBlog(blog);
                Results.Ok();
            });

            app.MapPut("/api/UpdateProject", async (ProjectPage project) =>
            {
                await dataWorker.UpdateProject(project);
                Results.Ok();
            });

            app.MapPut("/api/UpdateService", async (ServicePage service) =>
            {
                await dataWorker.UpdateService(service);
                Results.Ok();
            });

            app.MapPut("/api/UpdateContact", async (ContactPage contact) =>
            {
                await dataWorker.UpdateContact(contact);
                Results.Ok();
            });

            app.MapPut("/api/UpdateSocialNetwork", async (SocialNetwork socialNetwork) =>
            {
                await dataWorker.UpdateSocialNetwork(socialNetwork);
                Results.Ok();
            });

            app.MapPut("/api/UpdateStatusRequest", async (Request request) =>
            {
                await dataWorker.UpdateStatusRequest(request);
                Results.Ok();
            });

            app.MapDelete("/api/DeleteProject/{id}", async (string id) =>
            {
                var needData = await dataWorker.GetConcreteProject(id);
                if (needData is null) return Results.NotFound(new { message = "Project not found" });
                await dataWorker.DeleteProject(needData.ProjectId);
                return Results.Ok();
            });

            app.MapDelete("/api/DeleteService/{id}", async (string id) =>
            {
                var needData = await dataWorker.GetConcreteService(id);
                if (needData is null) return Results.NotFound(new { message = "Service not found" });
                await dataWorker.DeleteService(needData.ServiceId);
                return Results.Ok();
            });

            app.MapDelete("/api/DeleteBlog/{id}", async (string id) =>
            {
                var needData = await dataWorker.GetConcreteBlog(id);
                if (needData is null) return Results.NotFound(new { message = "Blog not found" });
                await dataWorker.DeleteBlog(needData.BlogId);
                return Results.Ok();
            });

            app.MapDelete("/api/DeleteSocialNetwork/{id}", async (string id) =>
            {
                var needData = await dataWorker.GetConcreteSocialNetwork(id);
                if (needData is null) return Results.NotFound(new { message = "SocialNetwork not found" });
                await dataWorker.DeleteSocialNetwork(needData.SocialNetworkId);
                return Results.Ok();
            });


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.Run();
        }
    }
}