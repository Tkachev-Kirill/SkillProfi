using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillProfiBot
{
    public class DataInFile
    {
        public int Position { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }

        public DataInFile(DateTime date, int position)
        {
            Date = date;
            Position = position;
            Name = string.Empty;
            Email = string.Empty;
            Text = string.Empty;
        }
        public DataInFile()
        {
        }

        }
}
