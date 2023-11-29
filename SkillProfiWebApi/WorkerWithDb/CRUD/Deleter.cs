using Microsoft.EntityFrameworkCore;
using SkillProfiWebApi.WorkerWithDb.Context;

namespace SkillProfiWebApi.WorkerWithDb.CRUD
{
    public class Deleter
    {
        private ModelContext Context { get; set; }
        public Deleter(string strCon)
        {
            DbContextOptionsBuilder<ModelContext> optionsBuilder = new DbContextOptionsBuilder<ModelContext>();
            Context = new ModelContext(optionsBuilder.UseSqlServer(strCon).Options);
        }

        public async Task DeleteProject(int id)
        {
            var data = await Context.ProjectPages.FindAsync(id);
            if (data is null)
            {
                return;
            }
            Context.ProjectPages.Remove(data);
            await Context.SaveChangesAsync();
        }
        public async Task DeleteService(int id)
        {
            var data = await Context.ServicePages.FindAsync(id);
            if (data is null)
            {
                return;
            }
            Context.ServicePages.Remove(data);
            await Context.SaveChangesAsync();
        }
        public async Task DeleteBlog(int id)
        {
            var data = await Context.BlogPages.FindAsync(id);
            if (data is null)
            {
                return;
            }
            Context.BlogPages.Remove(data);
            await Context.SaveChangesAsync();
        }
        public async Task DeleteSocialNetwork(int id)
        {
            var data = await Context.SocialNetworks.FindAsync(id);
            if (data is null)
            {
                return;
            }
            Context.SocialNetworks.Remove(data);
            await Context.SaveChangesAsync();
        }
    }
}
