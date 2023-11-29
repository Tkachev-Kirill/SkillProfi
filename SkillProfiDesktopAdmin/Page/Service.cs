using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SkillProfiDesktopAdmin.Page
{
    internal class Service
    {
        public List<UIElement> ElementsService = new List<UIElement>();
        public ComboBox AllDataService = new ComboBox();
        public StackPanel FormAdd = new StackPanel();
        public TextBlock BlockName = new TextBlock();
        public TextBox NewBoxName = new TextBox();
        public TextBlock BlockText = new TextBlock();
        public TextBox NewBoxText = new TextBox();
        public List<UIElement> ElementsForDelete = new List<UIElement>();
    }
}
