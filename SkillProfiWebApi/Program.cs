using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidIssuer = builder.Configuration["Jwt:Issuer"],
                     ValidateAudience = true,
                     ValidAudience = builder.Configuration["Jwt:Audience"],
                     ValidateLifetime = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                     ValidateIssuerSigningKey = true,
                 };
             });
            builder.Services.AddAuthorization();


            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            IDataWorker dataWorker = GetDataFromSettingsHelper.GetDataFromSettings(builder.Configuration);

            app.MapGet("/api/Autentefications/{login}/{password}", async (string login, string password) =>
            {
                var success = await dataWorker.Autentefications(login, password);
                if (!string.IsNullOrEmpty(success))
                {
                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, login) };
                    // создаем JWT-токен
                    var jwt = new JwtSecurityToken(
                            issuer: builder.Configuration["Jwt:Issuer"],
                            audience: builder.Configuration["Jwt:Audience"],
                            claims: claims,
                            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(20)),
                           signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha256));

                    return Results.Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
                }
                return Results.Unauthorized();
            });

            app.MapGet("/api/GetImage/{nameFile}",  async (string nameFile, IConfiguration configuration) =>
            {
                var path = configuration.GetSection("NameFolder").Value;
                string needFolder = Path.Combine(@"C:\", path);
                string filePath = Path.Combine(needFolder, nameFile);
                return Results.File(filePath, "image/jpeg");
            });

            app.MapGet("/api/GetMainPage", () =>
            {
                return dataWorker.GetMainPage();
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


            app.MapGet("/api/GetDataAllRequest", [Authorize] () =>
            {
                return dataWorker.GetDataAllRequest();
            });
            app.MapGet("/api/GetDataAllRequestInDate/{dateStart}/{dateFinish}", [Authorize]  async (DateTime dateStart, DateTime dateFinish) =>
            {
                if (dateStart == default(DateTime))
                {
                    return Results.BadRequest(new { message = "Invalid date format" });
                }
                var needData = await dataWorker.GetDataAllRequestInDate(dateStart, dateFinish);
                if (needData is null) return Results.NotFound(new { message = "Request not found" });
                return Results.Json(needData);
            });

            app.MapPost("/api/UploadImage", [Authorize] async (HttpRequest request, IConfiguration configuration) =>
            {
                var form = await request.ReadFormAsync();
                var file = form.Files["file"];

                if (file.Length > 0)
                {
                    string newName = Guid.NewGuid().ToString() + ".jpeg";
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

            app.MapPost("/api/CreateNetwork", [Authorize] async (SocialNetwork network) =>
            {
                await dataWorker.CreateNetwork(network);
                return Results.Ok();
            });

            app.MapPost("/api/CreateNewAccount", [Authorize] async (Account acc) =>
            {
                await dataWorker.CreateNewAccount(acc);
                return Results.Ok();
            });

            app.MapPost("/api/CreateNewRequest", [Authorize] async (Request request) =>
            {
                await dataWorker.CreateNewRequest(request);
                Results.Ok();
            });

            app.MapPost("/api/CreateProject", [Authorize] async (ProjectPage project) =>
            {
                await dataWorker.CreateProject(project);
                Results.Ok();
            });

            app.MapPost("/api/CreateService", [Authorize] async (ServicePage service) =>
            {
                await dataWorker.CreateService(service);
                Results.Ok();
            });

            app.MapPost("/api/CreateBlog", [Authorize] async (BlogPage blog) =>
            {
                await dataWorker.CreateBlog(blog);
                Results.Ok();
            });

            app.MapPut("/api/UpdateMainPage", [Authorize] async (MainPage mainPage) =>
            {
                await dataWorker.UpdateMainPage(mainPage);
                Results.Ok();
            });

            app.MapPut("/api/UpdateBlog", [Authorize] async (BlogPage blog) =>
            {
                await dataWorker.UpdateBlog(blog);
                Results.Ok();
            });

            app.MapPut("/api/UpdateProject", [Authorize] async (ProjectPage project) =>
            {
                await dataWorker.UpdateProject(project);
                Results.Ok();
            });

            app.MapPut("/api/UpdateService", [Authorize] async (ServicePage service) =>
            {
                await dataWorker.UpdateService(service);
                Results.Ok();
            });

            app.MapPut("/api/UpdateContact", [Authorize] async (ContactPage contact) =>
            {
                await dataWorker.UpdateContact(contact);
                Results.Ok();
            });

            app.MapPut("/api/UpdateSocialNetwork", [Authorize] async (SocialNetwork socialNetwork) =>
            {
                await dataWorker.UpdateSocialNetwork(socialNetwork);
                Results.Ok();
            });

            app.MapPut("/api/UpdateStatusRequest", [Authorize] async (Request request) =>
            {
                await dataWorker.UpdateStatusRequest(request);
                Results.Ok();
            });

            app.MapDelete("/api/DeleteProject/{id}", [Authorize] async (string id) =>
            {
                var needData = await dataWorker.GetConcreteProject(id);
                if (needData is null) return Results.NotFound(new { message = "Project not found" });
                await dataWorker.DeleteProject(needData.ProjectId);
                return Results.Ok();
            });

            app.MapDelete("/api/DeleteService/{id}", [Authorize] async (string id) =>
            {
                var needData = await dataWorker.GetConcreteService(id);
                if (needData is null) return Results.NotFound(new { message = "Service not found" });
                await dataWorker.DeleteService(needData.ServiceId);
                return Results.Ok();
            });

            app.MapDelete("/api/DeleteBlog/{id}", [Authorize] async (string id) =>
            {
                var needData = await dataWorker.GetConcreteBlog(id);
                if (needData is null) return Results.NotFound(new { message = "Blog not found" });
                await dataWorker.DeleteBlog(needData.BlogId);
                return Results.Ok();
            });

            app.MapDelete("/api/DeleteSocialNetwork/{id}", [Authorize] async (string id) =>
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