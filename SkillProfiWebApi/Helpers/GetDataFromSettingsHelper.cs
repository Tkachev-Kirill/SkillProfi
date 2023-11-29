using SkillProfiClasses.Interface;
using SkillProfiWebApi.Fabric;

namespace SkillProfiWebApi.Helpers
{
    public static class GetDataFromSettingsHelper
    {
        public static IDataWorker GetDataFromSettings(ConfigurationManager configuration)
        {
            string needCon = configuration.GetSection("TypeConnectNeed").Value;

            string connection = configuration.GetConnectionString(needCon);

            return DataWorkerFactory.CreateDataWorker(needCon, connection);

        }
    }
}
