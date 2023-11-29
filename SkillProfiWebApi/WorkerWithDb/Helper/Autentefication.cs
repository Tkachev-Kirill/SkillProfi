using Microsoft.EntityFrameworkCore;
using SkillProfiWebApi.WorkerWithDb.Context;

namespace SkillProfiWebApi.WorkerWithDb.Helper
{
    public static class Autentefication
    {
        public static async Task<bool> CheckValidLogAndPass(string login, string password, ModelContext context)
        {
            var account = await context.Accounts.Where(a => a.Login == login && a.Password == password).FirstOrDefaultAsync();
            if (account is null)
            {
                return false;
            }
            return true;
        }
    }
}
