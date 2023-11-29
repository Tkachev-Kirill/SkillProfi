using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillProfiClasses.RequestData
{
    public static class ReadyLists
    {
        public static Dictionary<int,string> GetAllTypeRequest()
        {
            return new Dictionary<int,string> {
                { 0, "Telegram" },
                { 1, "Site" }};
        }
        public static Dictionary<int, string> GetAllStatusRequest()
        {
            return new Dictionary<int, string> {
                { 0, "Получена" },
                { 1, "В работе" },
                { 2, "Выполнена" },
                { 3, "Отклонена" },
                { 4, "Отменена" }};
        }
    }
}
