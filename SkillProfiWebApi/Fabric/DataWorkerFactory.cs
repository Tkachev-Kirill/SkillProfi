using SkillProfiClasses.Interface;
using SkillProfiWebApi.DataWorkers;

namespace SkillProfiWebApi.Fabric
{
    public static class DataWorkerFactory
    {
        public static IDataWorker CreateDataWorker(string typeConnectNeed, string connection)
        {
            switch (typeConnectNeed)
            {
                case "DbConnection":
                    return new DataWorkerDb(connection);
                default:
                    return new DataWorkerDb(connection);
            }
        }
    }
}
