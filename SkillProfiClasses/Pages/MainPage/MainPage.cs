using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillProfiClasses.Pages.MainPage
{
    public class MainPage
    {
        [Key]
        public int MainId { get; set; }
        public string NameBaner { get; set; }
        public string TextInBaner { get; set; }
        public string TextUnderBaner { get; set; }
    }
}
