
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SkillProfiClasses.RequestData
{
    public class Request
    {
        public int RequestId { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
        public int RequestTypeNum { get; set; }
        public int RequestStatusNum { get; set; }

        [NotMapped]
        public string NameType
        {
            get
            {
                switch (RequestTypeNum)
                {
                    case 0:
                        return "Telegram";
                    case 1:
                        return "Site";
                    default:
                        return "Unknown";
                }
            }
        }
        [NotMapped]
        public string NameStatus
        {
            get
            {
                switch (RequestStatusNum)
                {
                    case 0:
                        return "Получена";
                    case 1:
                        return "В работе";
                    case 2:
                        return "Выполнена";
                    case 3:
                        return "Отклонена";
                    case 4:
                        return "Отменена";
                    default:
                        return "Unknown";
                }
            }
        }
    }
}
