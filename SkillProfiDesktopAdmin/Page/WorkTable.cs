using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace SkillProfiDesktopAdmin.Page
{
    internal class WorkTable
    {
        public List<UIElement> ElementsWorkTable = new List<UIElement>();
        public ScrollViewer ScrollViewer = new ScrollViewer();
        public DatePicker DateStartPicker = new DatePicker();
        public DatePicker DateFinishPicker = new DatePicker();
        public TextBlock CountAllRequest = new TextBlock();
        public TextBlock Id = new TextBlock();
        public ComboBox Status = new ComboBox();
        public ListBox AllRequest = new ListBox();
    }
}
