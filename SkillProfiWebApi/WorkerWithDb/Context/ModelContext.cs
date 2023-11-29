using Microsoft.EntityFrameworkCore;
using SkillProfiClasses.AccountData;
using SkillProfiClasses.Pages.BlogPage;
using SkillProfiClasses.Pages.ContactPage;
using SkillProfiClasses.Pages.MainPage;
using SkillProfiClasses.Pages.ProjectPage;
using SkillProfiClasses.Pages.ServicePage;
using SkillProfiClasses.RequestData;
using System.Runtime.Serialization;

namespace SkillProfiWebApi.WorkerWithDb.Context
{
    public class ModelContext : DbContext
    {
        public DbSet<BlogPage> BlogPages { get; set; }
        public DbSet<ContactPage> ContactPages { get; set; }
        public DbSet<SocialNetwork> SocialNetworks { get; set; }
        public DbSet<MainPage> MainPages { get; set; }
        public DbSet<ProjectPage> ProjectPages { get; set; }
        public DbSet<ServicePage> ServicePages { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MainPage>().HasData(new MainPage
            {
                MainId = 1,
                NameBaner = "Test",
                TextInBaner = "Test",
                TextUnderBaner = "Test"
            });

            modelBuilder.Entity<Account>().HasData(new Account
            {
                Accountid = 1,
                Login = "admin",
                Password = "admin"
            });

            modelBuilder.Entity<ContactPage>().HasData(new ContactPage
            {
                ContactId = 1,
                Phone = "Test",
                Address = "Test",
                Email = "Test"
            });

        }

        public ModelContext(DbContextOptions<ModelContext> options)
          : base(options)
        {
            Database.EnsureCreated();
        }
    }

  
}
