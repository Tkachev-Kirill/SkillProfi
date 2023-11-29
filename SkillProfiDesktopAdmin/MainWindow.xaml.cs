using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SkillProfiDesktopAdmin.Page;
using SkillProfiDesktopAdmin.Workers;

namespace SkillProfiDesktopAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        private async void ButtonAuth_Click(object sender, RoutedEventArgs e)
        {
            var Worker = new DesktopWorker();
            var log = Login.Text;
            var pass = Password.Password;
            bool success = await Worker.Autentefications(log, pass);
            if (success)
            {
                var adminWindow = new AdminWindow(log);
                this.Hide();
                adminWindow.Show();
            }
            else
            {
                MessageBox.Show("Пользователь с таким логином и паролем не найден", "Eror", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
