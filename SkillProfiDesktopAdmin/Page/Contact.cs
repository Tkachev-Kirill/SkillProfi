using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SkillProfiDesktopAdmin.Page
{
    internal class Contact
    {
        public List<UIElement> ElementsContact = new List<UIElement>();
        public ComboBox AllDataContact = new ComboBox();
        public StackPanel FormAddOrChange = new StackPanel();
        public TextBlock BlockName = new TextBlock();
        public TextBlock NewBlockName = new TextBlock();
        public TextBlock BlockImage = new TextBlock();
        public TextBlock NewBlockImage = new TextBlock();
        public System.Windows.Controls.Image MyImage = new System.Windows.Controls.Image();
        public List<UIElement> ElementsForDelete = new List<UIElement>();
    }
}
