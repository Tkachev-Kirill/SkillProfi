using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillProfiClasses.Pages.BlogPage
{
    public class BlogPage
    {
        [Key]
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string PathToImage { get; set; }
    }
}
