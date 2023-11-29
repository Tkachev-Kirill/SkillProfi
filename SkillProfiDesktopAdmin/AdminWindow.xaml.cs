using Microsoft.Win32;
using SkillProfiClasses.Pages.BlogPage;
using SkillProfiClasses.Pages.ContactPage;
using SkillProfiClasses.Pages.MainPage;
using SkillProfiClasses.Pages.ProjectPage;
using SkillProfiClasses.Pages.ServicePage;
using SkillProfiClasses.RequestData;
using SkillProfiDesktopAdmin.Page;
using SkillProfiDesktopAdmin.Workers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SkillProfiDesktopAdmin
{
    public partial class AdminWindow : Window
    {
        private DesktopWorker Worker;

        private WorkTable WorkTableMenu = new WorkTable();
        bool NowWindowIsWorkTable = false;

        private Main MainMenu = new Main();
        bool NowWindowIsMain = false;

        private Project ProjectMenu = new Project();
        bool NowWindowIsProject = false;

        private Service ServiceMenu = new Service();
        bool NowWindowIsService = false;

        private Blog BlogMenu = new Blog();
        bool NowWindowIsBlog = false;

        private Contact ContactMenu = new Contact();
        bool NowWindowIsContact = false;

        public AdminWindow(string login)
        {
            Worker = new DesktopWorker();
            InitializeComponent();
            AddLoginInHeader(login);
        }

        private void AddLoginInHeader(string login)
        {
            var textBloc = new TextBlock();
            textBloc.Text = login;
            Grid.SetRow(textBloc, 0);
            Grid.SetColumn(textBloc, 18);
            textBloc.Name = "ShowPhoneBook";
            allGrid.Children.Add(textBloc);
        }

        #region  WorkTable
        private void Button_ClickDesktop(object sender, RoutedEventArgs e)
        {
            CrateAndAddDataAllElementsWorkTable();
        }

        private void CrateAndAddDataAllElementsWorkTable()
        {
            if (NowWindowIsWorkTable)
            {
                return;
            }
            NowWindowIsWorkTable = true;

            ClearAllDataContact();
            ClearAllDataBlog();
            ClearAllDataProject();
            ClearAllDataService();
            ClearAllDataMain();

            CreateAllElementsWorkTable();
        }

        private async void CreateAllElementsWorkTable()
        {
            var allRequest = await Worker.GetDataAllRequest();
            var countAllRequest = new TextBlock();
            Grid.SetColumn(countAllRequest, 3);
            Grid.SetRow(countAllRequest, 0);
            Grid.SetColumnSpan(countAllRequest, 2);
            countAllRequest.VerticalAlignment = VerticalAlignment.Center;
            countAllRequest.HorizontalAlignment = HorizontalAlignment.Center;
            countAllRequest.Text = $"Всего {allRequest.Count} заявки";
            allGrid.Children.Add(countAllRequest);
            WorkTableMenu.ElementsWorkTable.Add(countAllRequest);

            var button = new Button();
            button.Content = "Сегодня";
            Grid.SetRow(button, 1);
            Grid.SetColumn(button, 3);
            Grid.SetColumnSpan(button, 2);
            button.Margin = new Thickness(0, 0, 10, 0);
            button.Click += new RoutedEventHandler(Button_ShowToday);
            allGrid.Children.Add(button);
            WorkTableMenu.ElementsWorkTable.Add(button);

            button = new Button();
            button.Content = "Вчера";
            Grid.SetRow(button, 1);
            Grid.SetColumn(button, 5);
            Grid.SetColumnSpan(button, 2);
            button.Margin = new Thickness(0, 0, 10, 0);
            button.Click += new RoutedEventHandler(Button_ShowYesterday);
            allGrid.Children.Add(button);
            WorkTableMenu.ElementsWorkTable.Add(button);

            button = new Button();
            button.Content = "Неделя";
            Grid.SetRow(button, 1);
            Grid.SetColumn(button, 7);
            Grid.SetColumnSpan(button, 2);
            button.Margin = new Thickness(0, 0, 10, 0);
            button.Click += new RoutedEventHandler(Button_ShowWeek);
            allGrid.Children.Add(button);
            WorkTableMenu.ElementsWorkTable.Add(button);

            button = new Button();
            button.Content = "Месяц";
            Grid.SetRow(button, 1);
            Grid.SetColumn(button, 9);
            Grid.SetColumnSpan(button, 2);
            button.Margin = new Thickness(0, 0, 10, 0);
            button.Click += new RoutedEventHandler(Button_ShowMounth);
            allGrid.Children.Add(button);
            WorkTableMenu.ElementsWorkTable.Add(button);


            Grid.SetRow(WorkTableMenu.DateStartPicker, 1);
            Grid.SetColumn(WorkTableMenu.DateStartPicker, 11);
            Grid.SetColumnSpan(WorkTableMenu.DateStartPicker, 3);
            WorkTableMenu.DateStartPicker.Margin = new Thickness(0, 0, 10, 0);
            allGrid.Children.Add(WorkTableMenu.DateStartPicker);
            WorkTableMenu.ElementsWorkTable.Add(WorkTableMenu.DateStartPicker);

            Grid.SetRow(WorkTableMenu.DateFinishPicker, 1);
            Grid.SetColumn(WorkTableMenu.DateFinishPicker, 14);
            Grid.SetColumnSpan(WorkTableMenu.DateFinishPicker, 3);
            WorkTableMenu.DateFinishPicker.Margin = new Thickness(0, 0, 10, 0);
            allGrid.Children.Add(WorkTableMenu.DateFinishPicker);
            WorkTableMenu.ElementsWorkTable.Add(WorkTableMenu.DateFinishPicker);

            button = new Button();
            button.Content = "Найти";
            Grid.SetRow(button, 1);
            Grid.SetColumn(button, 17);
            Grid.SetColumnSpan(button, 2);
            button.Margin = new Thickness(0, 0, 10, 0);
            button.Click += new RoutedEventHandler(Button_ShowPeriod);
            allGrid.Children.Add(button);
            WorkTableMenu.ElementsWorkTable.Add(button);
        }

        private void Button_ShowPeriod(object sender, RoutedEventArgs e)
        {

            if (WorkTableMenu.DateStartPicker == null || WorkTableMenu.DateFinishPicker == null || 
                !WorkTableMenu.DateStartPicker.SelectedDate.HasValue || !WorkTableMenu.DateFinishPicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Не выбран период для поиска!", "Eror", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (WorkTableMenu.DateStartPicker.SelectedDate.Value> WorkTableMenu.DateFinishPicker.SelectedDate.Value)
            {
                MessageBox.Show("Дата начала периода больше конца!", "Eror", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ShowRequests(WorkTableMenu.DateStartPicker.SelectedDate.Value, WorkTableMenu.DateFinishPicker.SelectedDate.Value);
        }

        private void Button_ShowMounth(object sender, RoutedEventArgs e)
        {
            DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 1);
            DateTime finish = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month), 23, 59, 59); 
            ShowRequests(start, finish);
        }

        private void Button_ShowWeek(object sender, RoutedEventArgs e)
        {
            DateTime start = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + 1).Date.AddSeconds(1);
            DateTime finish = DateTime.Now.AddDays(7 - (int)DateTime.Now.DayOfWeek).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            ShowRequests(start, finish);
        }

        private async void Button_ShowYesterday(object sender, RoutedEventArgs e)
        {
            DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day -1, 0, 0, 1);
            DateTime finish = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day-1, 23, 59, 59);
            ShowRequests(start, finish);
        }

        private async void Button_ShowToday(object sender, RoutedEventArgs e)
        {
            DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 1);
            DateTime finish = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            ShowRequests(start, finish);
        }

        private async void ShowRequests(DateTime dateStart, DateTime dateFinish)
        {
            var allRequest = await Worker.GetDataAllRequestInDate(dateStart, dateFinish);
            if (!allGrid.Children.Contains(WorkTableMenu.CountAllRequest))
            {
                Grid.SetColumn(WorkTableMenu.CountAllRequest, 3);
                Grid.SetRow(WorkTableMenu.CountAllRequest, 2);
                Grid.SetColumnSpan(WorkTableMenu.CountAllRequest, 5);
                WorkTableMenu.CountAllRequest.VerticalAlignment = VerticalAlignment.Center;
                WorkTableMenu.CountAllRequest.HorizontalAlignment = HorizontalAlignment.Center;
                WorkTableMenu.CountAllRequest.Text = $"За указанный период поступило: {allRequest.Count}";
                allGrid.Children.Add(WorkTableMenu.CountAllRequest);
                WorkTableMenu.ElementsWorkTable.Add(WorkTableMenu.CountAllRequest);
            }
            else
            {
                WorkTableMenu.CountAllRequest.Text = $"За указанный период поступило: {allRequest.Count}";
            }

            if (!allGrid.Children.Contains(WorkTableMenu.AllRequest))
            {
                Grid.SetColumn(WorkTableMenu.AllRequest, 3);
                Grid.SetRow(WorkTableMenu.AllRequest, 3);
                Grid.SetColumnSpan(WorkTableMenu.AllRequest, 16);
                Grid.SetRowSpan(WorkTableMenu.AllRequest, 9);
                WorkTableMenu.AllRequest.Margin = new Thickness(0, 0, 10, 0);

                AddAllStackPanel(allRequest);
                allGrid.Children.Add(WorkTableMenu.AllRequest);
                WorkTableMenu.ElementsWorkTable.Add(WorkTableMenu.AllRequest);
            }
            else
            {
                WorkTableMenu.AllRequest.Items.Clear();
                AddAllStackPanel(allRequest);
            }

        }
        private void AddAllStackPanel(List<Request> allRequest)
        {
            foreach (var req in allRequest)
            {
                StackPanel stackPanel = new StackPanel(); ;
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.Margin = new Thickness(20, 0, 0, 5);

                TextBlock id = new TextBlock();
                id.Text = req.RequestId.ToString();
                id.Margin = new Thickness(0, 0, 30, 0);
                id.Width = 10;

                TextBlock date = new TextBlock();
                date.Text = req.Date.ToString("dd.MM.yyyy HH:mm:ss");
                date.Margin = new Thickness(0, 0, 30, 0);
                date.Width = 110;

                TextBlock name = new TextBlock();
                name.Text = req.Name;
                name.Margin = new Thickness(0, 0, 30, 0);
                name.Width = 120;

                TextBlock text = new TextBlock();
                text.Text = req.Text;
                text.Margin = new Thickness(0, 0, 30, 0);
                text.Width = 310;

                TextBlock email = new TextBlock();
                email.Text = req.Email;
                email.Margin = new Thickness(0, 0,30, 0);
                email.Width = 120;

                ComboBox status = new ComboBox();
                var allStatus = ReadyLists.GetAllStatusRequest();
                foreach (var item in allStatus)
                {
                    status.Items.Add(item.Value);
                }
                status.SelectedIndex = req.RequestStatusNum;
                status.Margin = new Thickness(0, 0, 0, 0);
                status.Width = 110;

                status.SelectionChanged += (sender, e) =>
                {
                    // Получаем выбранный статус
                    var selectedStatus = (sender as ComboBox).SelectedItem as string;

                    // Обновляем статус в базе данных
                    var newRequest = new Request()
                    {
                        RequestId = req.RequestId,
                        Date = req.Date,
                        Email = req.Email,
                        Name = req.Name,
                        RequestTypeNum = req.RequestTypeNum,
                        RequestStatusNum = allStatus.FirstOrDefault(x => x.Value == selectedStatus).Key
                    };
                
                    Worker.UpdateStatusRequest(newRequest);

                    MessageBox.Show($"Изменен статус заявки с номером {req.RequestId}", "Измененен статус", MessageBoxButton.OK, MessageBoxImage.Information);
                };

                TextBlock type = new TextBlock();
                type.Text = req.NameType;
                type.Margin = new Thickness(0, 0, 30, 0);
                type.Width = 60;

                // Добавляем TextBlock и ComboBox в StackPanel
                stackPanel.Children.Add(id);
                stackPanel.Children.Add(date);
                stackPanel.Children.Add(name);
                stackPanel.Children.Add(text);
                stackPanel.Children.Add(email);
                stackPanel.Children.Add(type);
                stackPanel.Children.Add(status);
                WorkTableMenu.AllRequest.Items.Add(stackPanel);
            }
        }
        #endregion

        #region MainPage
        private void Button_ClickMain(object sender, RoutedEventArgs e)
        {
            CrateAndAddDataAllElementsMain();
        }

        private async void CrateAndAddDataAllElementsMain()
        {
            if (NowWindowIsMain)
            {
                return;
            }

            NowWindowIsMain = true;

            ClearAllDataWorkTable();
            ClearAllDataContact();
            ClearAllDataBlog();
            ClearAllDataProject();
            ClearAllDataService();

            var baner = new TextBlock();
            Grid.SetColumn(baner, 3);
            Grid.SetRow(baner, 1);
            Grid.SetColumnSpan(baner, 3);
            baner.VerticalAlignment = VerticalAlignment.Center;
            baner.HorizontalAlignment = HorizontalAlignment.Center;
            baner.Text = $"Название банера:";
            baner.Margin = new Thickness(0, 0, 10, 0);
            allGrid.Children.Add(baner);
            MainMenu.ElementsMain.Add(baner);

            var mainPage = await Worker.GetMainPage();

            var newBaner = new TextBox();
            Grid.SetColumn(newBaner, 7);
            Grid.SetRow(newBaner, 1);
            Grid.SetColumnSpan(newBaner, 3);
            newBaner.VerticalAlignment = VerticalAlignment.Center;
            newBaner.HorizontalAlignment = HorizontalAlignment.Center;
            newBaner.Text = mainPage.NameBaner;
            newBaner.Width = 150;
            newBaner.Height = 20;
            allGrid.Children.Add(newBaner);
            MainMenu.ElementsMain.Add(newBaner);

            var textInBaner = new TextBlock();
            Grid.SetColumn(textInBaner, 3);
            Grid.SetRow(textInBaner, 2);
            Grid.SetColumnSpan(textInBaner, 3);
            textInBaner.VerticalAlignment = VerticalAlignment.Center;
            textInBaner.HorizontalAlignment = HorizontalAlignment.Center;
            textInBaner.Text = $"Текст в банере:";
            textInBaner.Margin = new Thickness(0, 0, 10, 0);
            allGrid.Children.Add(textInBaner);
            MainMenu.ElementsMain.Add(textInBaner);

            var newTextInBaner = new TextBox();
            Grid.SetColumn(newTextInBaner, 7);
            Grid.SetRow(newTextInBaner, 2);
            Grid.SetColumnSpan(newTextInBaner, 3);
            newTextInBaner.VerticalAlignment = VerticalAlignment.Center;
            newTextInBaner.HorizontalAlignment = HorizontalAlignment.Center;
            newTextInBaner.Text = mainPage.TextInBaner;
            newTextInBaner.Width = 150;
            newTextInBaner.Height = 20;
            allGrid.Children.Add(newTextInBaner);
            MainMenu.ElementsMain.Add(newTextInBaner);

            var textUnderBaner = new TextBlock();
            Grid.SetColumn(textUnderBaner, 3);
            Grid.SetRow(textUnderBaner, 3);
            Grid.SetColumnSpan(textUnderBaner, 3);
            textUnderBaner.VerticalAlignment = VerticalAlignment.Center;
            textUnderBaner.HorizontalAlignment = HorizontalAlignment.Center;
            textUnderBaner.Text = $"Текст под банером:";
            textUnderBaner.Margin = new Thickness(0, 0, 10, 0);
            allGrid.Children.Add(textUnderBaner);
            MainMenu.ElementsMain.Add(textUnderBaner);

            var newTextUnderBaner = new TextBox();
            Grid.SetColumn(newTextUnderBaner, 7);
            Grid.SetRow(newTextUnderBaner, 3);
            Grid.SetColumnSpan(newTextUnderBaner, 3);
            newTextUnderBaner.VerticalAlignment = VerticalAlignment.Center;
            newTextUnderBaner.HorizontalAlignment = HorizontalAlignment.Center;
            newTextUnderBaner.Text = mainPage.TextUnderBaner;
            newTextUnderBaner.Width = 150;
            newTextUnderBaner.Height = 20;
            allGrid.Children.Add(newTextUnderBaner);
            MainMenu.ElementsMain.Add(newTextUnderBaner);

            var button = new Button();
            button.Content = "Сохранить";
            Grid.SetRow(button, 4);
            Grid.SetColumn(button, 4);
            Grid.SetColumnSpan(button, 2);
            button.VerticalAlignment = VerticalAlignment.Center;
            button.HorizontalAlignment = HorizontalAlignment.Center;
            button.Margin = new Thickness(0, 0, 10, 0);
            button.Click += (sender, e) =>
            {
                var newMainPage = new MainPage() { NameBaner = newBaner.Text, TextInBaner = newTextInBaner.Text,
                    TextUnderBaner = newTextUnderBaner.Text };
                if (mainPage.NameBaner == newMainPage.NameBaner && mainPage.TextInBaner == newMainPage.TextInBaner && mainPage.TextUnderBaner == newMainPage.TextUnderBaner)
                {
                    MessageBox.Show($"Вы не изменили никаких данных!", "INFO", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Worker.UpdateMainPage(newMainPage);
                    MessageBox.Show($"Успешно сохранены новые данные на главной странице сайта!", "Измененены данные на главной странице", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            };
            allGrid.Children.Add(button);
            MainMenu.ElementsMain.Add(button);
        }
        #endregion

        #region Project

        private void Button_ClickProject(object sender, RoutedEventArgs e)
        {
            CrateAndAddDataAllElementsProject();
        }

        private async void CrateAndAddDataAllElementsProject()
        {
            if (NowWindowIsProject)
            {
                return;
            }

            NowWindowIsProject = true;

            ClearAllDataWorkTable();
            ClearAllDataContact();
            ClearAllDataBlog();
            ClearAllDataMain();
            ClearAllDataService();

            AddDataInAllDataProject();
           
        }

        private async void AddDataInAllDataProject()
        {
            var data = await Worker.GetDataAllProject();
            ProjectMenu.AllDataProject.Items.Clear();
            if (!allGrid.Children.Contains(ProjectMenu.AllDataProject))
            {
                Grid.SetRow(ProjectMenu.AllDataProject, 1);
                Grid.SetColumn(ProjectMenu.AllDataProject, 3);
                Grid.SetColumnSpan(ProjectMenu.AllDataProject, 5);
                ProjectMenu.AllDataProject.Margin = new Thickness(0, 0, 10, 0);
                foreach (var item in data)
                {
                    ProjectMenu.AllDataProject.Items.Add($"{item.ProjectId}. {item.Name}");
                }
                allGrid.Children.Add(ProjectMenu.AllDataProject);
                ProjectMenu.ElementsProject.Add(ProjectMenu.AllDataProject);
                ProjectMenu.AllDataProject.SelectionChanged += (sender, e) =>
                {
                    var selectedProject = (sender as ComboBox).SelectedItem as string;
                    ShowDataProject(selectedProject);
                };
            }
            else
            {
                foreach (var item in data)
                {
                    ProjectMenu.AllDataProject.Margin = new Thickness(0, 0, 10, 0);
                    ProjectMenu.AllDataProject.Items.Add($"{item.ProjectId}. {item.Name}");
                }
            }

            var button = new Button();
            button.Content = "Добавить";
            Grid.SetRow(button, 1);
            Grid.SetColumn(button, 10);
            Grid.SetColumnSpan(button, 2);
            button.Click += (sender, e) =>
            {
                CreateStackPanelForAddOrChange(true);
            };
            allGrid.Children.Add(button);
            ProjectMenu.ElementsProject.Add(button);
            button = new Button();
            button.Content = "Изменить";
            Grid.SetRow(button, 2);
            Grid.SetColumn(button, 10);
            Grid.SetColumnSpan(button, 2);
            button.Click += (sender, e) =>
            {
                if (ProjectMenu.AllDataProject.SelectedItem == null)
                {
                    MessageBox.Show($"Не выбран проект который надо изменить", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                CreateStackPanelForAddOrChange(false);
            };
            button.Margin = new Thickness(0, 10, 0, 0);
            allGrid.Children.Add(button);
            ProjectMenu.ElementsProject.Add(button);

            button = new Button();
            button.Content = "Удалить";
            Grid.SetRow(button, 3);
            Grid.SetColumn(button, 10);
            Grid.SetColumnSpan(button, 2);
            button.Click += DeleteProject;
            button.Margin = new Thickness(0, 10, 0, 0);
            allGrid.Children.Add(button);
            ProjectMenu.ElementsProject.Add(button);
        }

        private async void DeleteProject(object sender, RoutedEventArgs e)
        {
            if (ProjectMenu.AllDataProject.SelectedItem != null)
            {
                var nameProj = ProjectMenu.AllDataProject.SelectedItem.ToString();
                var id = nameProj.Split('.').FirstOrDefault();
                await Worker.DeleteProject(Convert.ToInt32(id));
                AddDataInAllDataProject();
                MessageBox.Show("Успешно удалено!", "Ok", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Не выбран проект который надо удалить!", "Eror", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ProjectMenu.FormAddOrChange.Children.Clear();
            allGrid.Children.Remove(ProjectMenu.FormAddOrChange);
            foreach (var item in ProjectMenu.ElementsForDelete)
            {
                allGrid.Children.Remove(item);
            }
            AddDataInAllDataProject();
        }

        private async void ShowDataProject(string selectedProject)
        {
            if (selectedProject is null)
            {
                return;
            }
            string id = selectedProject.Split('.').First();
            var proj = await Worker.GetConcreteProject(id);

            if (!allGrid.Children.Contains(ProjectMenu.BlockName))
            {
                ProjectMenu.BlockName.VerticalAlignment = VerticalAlignment.Top;
                ProjectMenu.BlockName.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetRow(ProjectMenu.BlockName, 3);
                Grid.SetColumn(ProjectMenu.BlockName, 3);
                Grid.SetColumnSpan(ProjectMenu.BlockName, 3);
                ProjectMenu.BlockName.Text = $"Название проекта";
                ProjectMenu.BlockName.Margin = new Thickness(0, 0, 10, 10);
                ProjectMenu.ElementsProject.Add(ProjectMenu.BlockName);
                ProjectMenu.ElementsForDelete.Add(ProjectMenu.BlockName);
                allGrid.Children.Add(ProjectMenu.BlockName);
            }

            if (!allGrid.Children.Contains(ProjectMenu.NewBlockName))
            {
                ProjectMenu.NewBlockName.VerticalAlignment = VerticalAlignment.Top;
                ProjectMenu.NewBlockName.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetRow(ProjectMenu.NewBlockName, 3);
                Grid.SetColumn(ProjectMenu.NewBlockName, 6);
                Grid.SetColumnSpan(ProjectMenu.NewBlockName, 2);
                ProjectMenu.ElementsProject.Add(ProjectMenu.NewBlockName);
                ProjectMenu.ElementsForDelete.Add(ProjectMenu.NewBlockName);
                allGrid.Children.Add(ProjectMenu.NewBlockName);
            }
            ProjectMenu.NewBlockName.Text = proj.Name;

            if (!allGrid.Children.Contains(ProjectMenu.BlockDescription))
            {
                ProjectMenu.BlockDescription.VerticalAlignment = VerticalAlignment.Top;
                ProjectMenu.BlockDescription.HorizontalAlignment = HorizontalAlignment.Left;
                ProjectMenu.BlockDescription.Text = $"Текст проекта";
                Grid.SetRow(ProjectMenu.BlockDescription, 4);
                Grid.SetColumn(ProjectMenu.BlockDescription, 3);
                Grid.SetColumnSpan(ProjectMenu.BlockDescription, 3);
                ProjectMenu.AllDataProject.Margin = new Thickness(0, 0, 10, 10);
                ProjectMenu.ElementsProject.Add(ProjectMenu.BlockDescription);
                ProjectMenu.ElementsForDelete.Add(ProjectMenu.BlockDescription);
                allGrid.Children.Add(ProjectMenu.BlockDescription);
            }

            if (!allGrid.Children.Contains(ProjectMenu.NewBlockDescription))
            {
                ProjectMenu.NewBlockDescription.VerticalAlignment = VerticalAlignment.Top;
                ProjectMenu.NewBlockDescription.HorizontalAlignment = HorizontalAlignment.Left;
                ProjectMenu.NewBlockDescription.TextWrapping = TextWrapping.Wrap;
                Grid.SetRow(ProjectMenu.NewBlockDescription, 4);
                Grid.SetColumn(ProjectMenu.NewBlockDescription, 6);
                Grid.SetColumnSpan(ProjectMenu.NewBlockDescription, 4);
                Grid.SetRowSpan(ProjectMenu.NewBlockDescription, 4);
                ProjectMenu.ElementsProject.Add(ProjectMenu.NewBlockDescription);
                ProjectMenu.ElementsForDelete.Add(ProjectMenu.NewBlockDescription);
                allGrid.Children.Add(ProjectMenu.NewBlockDescription);
            }
            ProjectMenu.NewBlockDescription.Text = proj.Description;

            if (!allGrid.Children.Contains(ProjectMenu.BlockImage))
            {
                ProjectMenu.BlockImage.VerticalAlignment = VerticalAlignment.Top;
                ProjectMenu.BlockImage.HorizontalAlignment = HorizontalAlignment.Left;
                ProjectMenu.BlockImage.Text = $"Картинка";
                Grid.SetRow(ProjectMenu.BlockImage, 8);
                Grid.SetColumn(ProjectMenu.BlockImage, 3);
                Grid.SetColumnSpan(ProjectMenu.BlockImage, 3);
                ProjectMenu.AllDataProject.Margin = new Thickness(0, 0, 10, 10);
                ProjectMenu.ElementsProject.Add(ProjectMenu.BlockImage);
                ProjectMenu.ElementsForDelete.Add(ProjectMenu.BlockImage);
                allGrid.Children.Add(ProjectMenu.BlockImage);
            }

            string needPath = await Worker.GetImage(proj.PathToImage);
            if (File.Exists(needPath))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(needPath, UriKind.Absolute);
                bitmap.EndInit();
                if (allGrid.Children.Contains(ProjectMenu.NewBlockImage))
                {
                    allGrid.Children.Remove(ProjectMenu.NewBlockImage);
                }
                if (!allGrid.Children.Contains(ProjectMenu.MyImage))
                {
                    Grid.SetRow(ProjectMenu.MyImage, 8);
                    Grid.SetColumn(ProjectMenu.MyImage, 6);
                    Grid.SetColumnSpan(ProjectMenu.MyImage, 4);
                    Grid.SetRowSpan(ProjectMenu.MyImage, 4);
                    ProjectMenu.ElementsProject.Add(ProjectMenu.MyImage);
                    ProjectMenu.ElementsForDelete.Add(ProjectMenu.MyImage);
                    allGrid.Children.Add(ProjectMenu.MyImage);
                }
                ProjectMenu.MyImage.Source = bitmap;

            }
            else
            {
                if (allGrid.Children.Contains(ProjectMenu.MyImage))
                {
                    allGrid.Children.Remove(ProjectMenu.MyImage);
                }
                if (!allGrid.Children.Contains(ProjectMenu.NewBlockImage))
                {
                    ProjectMenu.NewBlockImage.Text = "Картинки не существует";
                    Grid.SetRow(ProjectMenu.NewBlockImage, 8);
                    Grid.SetColumn(ProjectMenu.NewBlockImage, 6);
                    Grid.SetColumnSpan(ProjectMenu.NewBlockImage, 4);
                    ProjectMenu.ElementsProject.Add(ProjectMenu.NewBlockImage);
                    ProjectMenu.ElementsForDelete.Add(ProjectMenu.NewBlockImage);
                    allGrid.Children.Add(ProjectMenu.NewBlockImage);
                }
            }
        }

        private void CreateStackPanelForAddOrChange(bool needAdd)
        {
            if (!allGrid.Children.Contains(ProjectMenu.FormAddOrChange))
            {
                Grid.SetRow(ProjectMenu.FormAddOrChange, 1);
                Grid.SetColumn(ProjectMenu.FormAddOrChange, 13);
                Grid.SetColumnSpan(ProjectMenu.FormAddOrChange, 5);
                Grid.SetRowSpan(ProjectMenu.FormAddOrChange, 7);
                AddItemInStackPanel(needAdd);
                allGrid.Children.Add(ProjectMenu.FormAddOrChange);
                ProjectMenu.ElementsProject.Add(ProjectMenu.FormAddOrChange);
            }
            else
            {
                ProjectMenu.FormAddOrChange.Children.Clear();
                AddItemInStackPanel(needAdd);
            }
        }

        private async void AddItemInStackPanel(bool needAdd)
        {
            var proj = new ProjectPage();
            if (!needAdd)
            {
                var id = ProjectMenu.AllDataProject.SelectedItem.ToString().Split('.').First();
                proj = await Worker.GetConcreteProject(id);
            }
            var block = new TextBlock();
            block.VerticalAlignment = VerticalAlignment.Center;
            block.HorizontalAlignment = HorizontalAlignment.Center;
            block.Text = needAdd? $"Добавление проекта": $"Изменение проекта";
            ProjectMenu.AllDataProject.Margin = new Thickness(0, 0, 0, 20);
            ProjectMenu.FormAddOrChange.Children.Add(block);

            block = new TextBlock();
            block.VerticalAlignment = VerticalAlignment.Center;
            block.HorizontalAlignment = HorizontalAlignment.Center;
            block.Text = $"Название проекта";
            ProjectMenu.FormAddOrChange.Children.Add(block);

            var nameProject = new TextBox();
            nameProject.VerticalAlignment = VerticalAlignment.Center;
            nameProject.HorizontalAlignment = HorizontalAlignment.Center;
            ProjectMenu.FormAddOrChange.Children.Add(nameProject);
            nameProject.Width = 310;

            block = new TextBlock();
            block.VerticalAlignment = VerticalAlignment.Center;
            block.HorizontalAlignment = HorizontalAlignment.Center;
            block.Text = $"Описание проекта";
            ProjectMenu.FormAddOrChange.Children.Add(block);

            var descriptionProject = new TextBox();
            descriptionProject = new TextBox();
            descriptionProject.VerticalAlignment = VerticalAlignment.Center;
            descriptionProject.HorizontalAlignment = HorizontalAlignment.Center;
            ProjectMenu.FormAddOrChange.Children.Add(descriptionProject);
            descriptionProject.Width = 310;

            block = new TextBlock();
            block.VerticalAlignment = VerticalAlignment.Center;
            block.HorizontalAlignment = HorizontalAlignment.Center;
            block.Text = $"Картинка";
            ProjectMenu.FormAddOrChange.Children.Add(block);

            block = new TextBlock();
            block.VerticalAlignment = VerticalAlignment.Center;
            block.HorizontalAlignment = HorizontalAlignment.Center;

            if (!needAdd)
            {
                nameProject.Text = proj.Name;
                descriptionProject.Text = proj.Description;
                block.Text = proj.PathToImage;
            }

            var button = new Button();
            button.Content = "Добавить картинку";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            button.Click += (sender, e) =>
            {
                openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg";

                if (openFileDialog.ShowDialog() == true)
                    block.Text = openFileDialog.FileName;
            };
            ProjectMenu.FormAddOrChange.Children.Add(button);
            ProjectMenu.FormAddOrChange.Children.Add(block);

            button = new Button();
            button.Content = "Сохранить";
            
                button.Click += async (sender, e) =>
                {
                    string path = block.Text;
                    if (string.IsNullOrEmpty(path))
                    {
                        MessageBox.Show($"Вы не добавили картинку", "Eror", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (needAdd)
                    {
                        string name = await Worker.UploadFile(path); 
                        string newName = name.Replace("\"", "");
                        var newProject = new ProjectPage() { Name = nameProject.Text, Description = descriptionProject.Text, PathToImage = newName };
                        await Worker.CreateProject(newProject);
                        MessageBox.Show($"Успешно сохранен новый проект", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        string newName = proj.PathToImage;
                        if (proj.PathToImage!= path)
                        {
                            string name = await Worker.UploadFile(path);
                            newName = name.Replace("\"", "");
                        }
                        var newProject = new ProjectPage() {ProjectId = proj.ProjectId, Name = nameProject.Text, Description = descriptionProject.Text, PathToImage = newName };
                        await Worker.UpdateProject(newProject);
                        MessageBox.Show($"Успешно изменен проект №{proj.ProjectId}", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    ProjectMenu.FormAddOrChange.Children.Clear();
                    allGrid.Children.Remove(ProjectMenu.FormAddOrChange);
                    foreach (var item in ProjectMenu.ElementsForDelete)
                    {
                        allGrid.Children.Remove(item);
                    }
                    AddDataInAllDataProject();
                };
          
            ProjectMenu.FormAddOrChange.Children.Add(button);

        }
        #endregion

        #region Blog

        private void Button_Blog(object sender, RoutedEventArgs e)
        {
            CrateAndAddDataAllElementsBlog();
        }

        private async void CrateAndAddDataAllElementsBlog()
        {
            if (NowWindowIsBlog)
            {
                return;
            }

            NowWindowIsBlog = true;

            ClearAllDataWorkTable();
            ClearAllDataContact();
            ClearAllDataService();
            ClearAllDataMain();
            ClearAllDataProject();

            AddDataInAllDataBlog();

        }

        private async void AddDataInAllDataBlog()
        {
            var data = await Worker.GetDataAllBlog();
            BlogMenu.AllDataBlog.Items.Clear();
            if (!allGrid.Children.Contains(BlogMenu.AllDataBlog))
            {
                Grid.SetRow(BlogMenu.AllDataBlog, 1);
                Grid.SetColumn(BlogMenu.AllDataBlog, 3);
                Grid.SetColumnSpan(BlogMenu.AllDataBlog, 5);
                BlogMenu.AllDataBlog.Margin = new Thickness(0, 0, 10, 0);
                foreach (var item in data)
                {
                    BlogMenu.AllDataBlog.Items.Add($"{item.BlogId}. {item.Name}");
                }
                allGrid.Children.Add(BlogMenu.AllDataBlog);
                BlogMenu.ElementsBlog.Add(BlogMenu.AllDataBlog);
                BlogMenu.AllDataBlog.SelectionChanged += (sender, e) =>
                {
                    var selectedBlog = (sender as ComboBox).SelectedItem as string;
                    ShowDataBlog(selectedBlog);
                };
            }
            else
            {
                foreach (var item in data)
                {
                    BlogMenu.AllDataBlog.Margin = new Thickness(0, 0, 10, 0);
                    BlogMenu.AllDataBlog.Items.Add($"{item.BlogId}. {item.Name}");
                }
            }

            var button = new Button();
            button.Content = "Добавить";
            Grid.SetRow(button, 1);
            Grid.SetColumn(button, 10);
            Grid.SetColumnSpan(button, 2);
            button.Click += (sender, e) =>
            {
                CreateStackPanelForAddOrChangeBlog(true);
            };
            allGrid.Children.Add(button);
            BlogMenu.ElementsBlog.Add(button);
            button = new Button();
            button.Content = "Изменить";
            Grid.SetRow(button, 2);
            Grid.SetColumn(button, 10);
            Grid.SetColumnSpan(button, 2);
            button.Click += (sender, e) =>
            {
                if (BlogMenu.AllDataBlog.SelectedItem == null)
                {
                    MessageBox.Show($"Не выбран блог который надо изменить", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                CreateStackPanelForAddOrChangeBlog(false);
            };
            button.Margin = new Thickness(0, 10, 0, 0);
            allGrid.Children.Add(button);
            BlogMenu.ElementsBlog.Add(button);

            button = new Button();
            button.Content = "Удалить";
            Grid.SetRow(button, 3);
            Grid.SetColumn(button, 10);
            Grid.SetColumnSpan(button, 2);
            button.Click += DeleteBlog;
            button.Margin = new Thickness(0, 10, 0, 0);
            allGrid.Children.Add(button);
            BlogMenu.ElementsBlog.Add(button);
        }

        private async void DeleteBlog(object sender, RoutedEventArgs e)
        {
            if (BlogMenu.AllDataBlog.SelectedItem != null)
            {
                var nameBlog = BlogMenu.AllDataBlog.SelectedItem.ToString();
                var id = nameBlog.Split('.').FirstOrDefault();
                await Worker.DeleteBlog(Convert.ToInt32(id));
                AddDataInAllDataBlog();
                MessageBox.Show("Успешно удалено!", "Ok", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Не выбран проект который надо удалить!", "Eror", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            BlogMenu.FormAddOrChange.Children.Clear();
            allGrid.Children.Remove(BlogMenu.FormAddOrChange);
            foreach (var item in BlogMenu.ElementsForDelete)
            {
                allGrid.Children.Remove(item);
            }
            AddDataInAllDataBlog();
        }

        private async void ShowDataBlog(string selectedBlog)
        {
            if (selectedBlog is null)
            {
                return;
            }
            string id = selectedBlog.Split('.').First();
            var blog = await Worker.GetConcreteBlog(id);

            if (!allGrid.Children.Contains(BlogMenu.BlockName))
            {
                BlogMenu.BlockName.VerticalAlignment = VerticalAlignment.Top;
                BlogMenu.BlockName.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetRow(BlogMenu.BlockName, 3);
                Grid.SetColumn(BlogMenu.BlockName, 3);
                Grid.SetColumnSpan(BlogMenu.BlockName, 3);
                BlogMenu.BlockName.Text = $"Название блога";
                BlogMenu.BlockName.Margin = new Thickness(0, 0, 10, 10);
                BlogMenu.ElementsBlog.Add(BlogMenu.BlockName);
                BlogMenu.ElementsForDelete.Add(BlogMenu.BlockName);
                allGrid.Children.Add(BlogMenu.BlockName);
            }

            if (!allGrid.Children.Contains(BlogMenu.NewBlockName))
            {
                BlogMenu.NewBlockName.VerticalAlignment = VerticalAlignment.Top;
                BlogMenu.NewBlockName.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetRow(BlogMenu.NewBlockName, 3);
                Grid.SetColumn(BlogMenu.NewBlockName, 6);
                Grid.SetColumnSpan(BlogMenu.NewBlockName, 2);
                BlogMenu.ElementsBlog.Add(BlogMenu.NewBlockName);
                BlogMenu.ElementsForDelete.Add(BlogMenu.NewBlockName);
                allGrid.Children.Add(BlogMenu.NewBlockName);
            }
            BlogMenu.NewBlockName.Text = blog.Name;

            if (!allGrid.Children.Contains(BlogMenu.BlockDescription))
            {
                BlogMenu.BlockDescription.VerticalAlignment = VerticalAlignment.Top;
                BlogMenu.BlockDescription.HorizontalAlignment = HorizontalAlignment.Left;
                BlogMenu.BlockDescription.Text = $"Описание блога";
                Grid.SetRow(BlogMenu.BlockDescription, 4);
                Grid.SetColumn(BlogMenu.BlockDescription, 3);
                Grid.SetColumnSpan(BlogMenu.BlockDescription, 3);
                BlogMenu.ElementsBlog.Add(BlogMenu.BlockDescription);
                BlogMenu.ElementsForDelete.Add(BlogMenu.BlockDescription);
                allGrid.Children.Add(BlogMenu.BlockDescription);
            }

            if (!allGrid.Children.Contains(BlogMenu.NewBlockDescription))
            {
                BlogMenu.NewBlockDescription.VerticalAlignment = VerticalAlignment.Top;
                BlogMenu.NewBlockDescription.HorizontalAlignment = HorizontalAlignment.Left;
                BlogMenu.NewBlockDescription.TextWrapping = TextWrapping.Wrap;
                Grid.SetRow(BlogMenu.NewBlockDescription, 4);
                Grid.SetColumn(BlogMenu.NewBlockDescription, 6);
                Grid.SetColumnSpan(BlogMenu.NewBlockDescription, 4);
                Grid.SetRowSpan(BlogMenu.NewBlockDescription, 4);
                BlogMenu.ElementsBlog.Add(BlogMenu.NewBlockDescription);
                BlogMenu.ElementsForDelete.Add(BlogMenu.NewBlockDescription);
                allGrid.Children.Add(BlogMenu.NewBlockDescription);
            }
            BlogMenu.NewBlockDescription.Text = blog.Description;

            if (!allGrid.Children.Contains(BlogMenu.BlockText))
            {
                BlogMenu.BlockText.VerticalAlignment = VerticalAlignment.Top;
                BlogMenu.BlockText.HorizontalAlignment = HorizontalAlignment.Left;
                BlogMenu.BlockText.Text = $"Текст блога";
                Grid.SetRow(BlogMenu.BlockText, 5);
                Grid.SetColumn(BlogMenu.BlockText, 3);
                Grid.SetColumnSpan(BlogMenu.BlockText, 3);
                BlogMenu.ElementsBlog.Add(BlogMenu.BlockText);
                BlogMenu.ElementsForDelete.Add(BlogMenu.BlockText);
                allGrid.Children.Add(BlogMenu.BlockText);
            }

            if (!allGrid.Children.Contains(BlogMenu.NewBlockText))
            {
                BlogMenu.NewBlockText.VerticalAlignment = VerticalAlignment.Top;
                BlogMenu.NewBlockText.HorizontalAlignment = HorizontalAlignment.Left;
                BlogMenu.NewBlockText.TextWrapping = TextWrapping.Wrap;
                Grid.SetRow(BlogMenu.NewBlockText, 5);
                Grid.SetColumn(BlogMenu.NewBlockText, 6);
                Grid.SetColumnSpan(BlogMenu.NewBlockText, 4);
                Grid.SetRowSpan(BlogMenu.NewBlockText, 4);
                BlogMenu.ElementsBlog.Add(BlogMenu.NewBlockText);
                BlogMenu.ElementsForDelete.Add(BlogMenu.NewBlockText);
                allGrid.Children.Add(BlogMenu.NewBlockText);
            }
            BlogMenu.NewBlockText.Text = blog.Text;

            if (!allGrid.Children.Contains(BlogMenu.BlockImage))
            {
                BlogMenu.BlockImage.VerticalAlignment = VerticalAlignment.Top;
                BlogMenu.BlockImage.HorizontalAlignment = HorizontalAlignment.Left;
                BlogMenu.BlockImage.Text = $"Картинка";
                Grid.SetRow(BlogMenu.BlockImage, 8);
                Grid.SetColumn(BlogMenu.BlockImage, 3);
                Grid.SetColumnSpan(BlogMenu.BlockImage, 3);
                BlogMenu.ElementsBlog.Add(BlogMenu.BlockImage);
                BlogMenu.ElementsForDelete.Add(BlogMenu.BlockImage);
                allGrid.Children.Add(BlogMenu.BlockImage);
            }


            string fullPath = await Worker.GetImage(blog.PathToImage);
            if (File.Exists(fullPath))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(fullPath, UriKind.Absolute);
                bitmap.EndInit();
                if (allGrid.Children.Contains(BlogMenu.NewBlockImage))
                {
                    allGrid.Children.Remove(BlogMenu.NewBlockImage);
                }
                if (!allGrid.Children.Contains(BlogMenu.MyImage))
                {
                    Grid.SetRow(BlogMenu.MyImage, 8);
                    Grid.SetColumn(BlogMenu.MyImage, 6);
                    Grid.SetColumnSpan(BlogMenu.MyImage, 4);
                    Grid.SetRowSpan(BlogMenu.MyImage, 4);
                    BlogMenu.ElementsBlog.Add(BlogMenu.MyImage);
                    BlogMenu.ElementsForDelete.Add(BlogMenu.MyImage);
                    allGrid.Children.Add(BlogMenu.MyImage);
                }
                BlogMenu.MyImage.Source = bitmap;

            }
            else
            {
                if (allGrid.Children.Contains(BlogMenu.MyImage))
                {
                    allGrid.Children.Remove(BlogMenu.MyImage);
                }
                if (!allGrid.Children.Contains(BlogMenu.NewBlockImage))
                {
                    BlogMenu.NewBlockImage.Text = "Картинки не существует";
                    Grid.SetRow(BlogMenu.NewBlockImage, 8);
                    Grid.SetColumn(BlogMenu.NewBlockImage, 6);
                    Grid.SetColumnSpan(BlogMenu.NewBlockImage, 4);
                    BlogMenu.ElementsBlog.Add(BlogMenu.NewBlockImage);
                    BlogMenu.ElementsForDelete.Add(BlogMenu.NewBlockImage);
                    allGrid.Children.Add(BlogMenu.NewBlockImage);
                }
            }
        }

        private void CreateStackPanelForAddOrChangeBlog(bool needAdd)
        {
            if (!allGrid.Children.Contains(BlogMenu.FormAddOrChange))
            {
                Grid.SetRow(BlogMenu.FormAddOrChange, 1);
                Grid.SetColumn(BlogMenu.FormAddOrChange, 13);
                Grid.SetColumnSpan(BlogMenu.FormAddOrChange, 5);
                Grid.SetRowSpan(BlogMenu.FormAddOrChange, 9);
                AddItemInStackPanelBlog(needAdd);
                allGrid.Children.Add(BlogMenu.FormAddOrChange);
                BlogMenu.ElementsBlog.Add(BlogMenu.FormAddOrChange);
            }
            else
            {
                BlogMenu.FormAddOrChange.Children.Clear();
                AddItemInStackPanelBlog(needAdd);
            }
        }

        private async void AddItemInStackPanelBlog(bool needAdd)
        {
            var blog = new BlogPage();
            if (!needAdd)
            {
                var id = BlogMenu.AllDataBlog.SelectedItem.ToString().Split('.').First();
                blog = await Worker.GetConcreteBlog(id);
            }
            var block = new TextBlock();
            block.VerticalAlignment = VerticalAlignment.Center;
            block.HorizontalAlignment = HorizontalAlignment.Center;
            block.Text = needAdd ? $"Добавление блога" : $"Изменение блога";
            BlogMenu.AllDataBlog.Margin = new Thickness(0, 0, 0, 20);
            BlogMenu.FormAddOrChange.Children.Add(block);

            block = new TextBlock();
            block.VerticalAlignment = VerticalAlignment.Center;
            block.HorizontalAlignment = HorizontalAlignment.Center;
            block.Text = $"Название блога";
            BlogMenu.FormAddOrChange.Children.Add(block);

            var nameBlog = new TextBox();
            nameBlog.VerticalAlignment = VerticalAlignment.Center;
            nameBlog.HorizontalAlignment = HorizontalAlignment.Center;
            BlogMenu.FormAddOrChange.Children.Add(nameBlog);
            nameBlog.Width = 310;

            block = new TextBlock();
            block.VerticalAlignment = VerticalAlignment.Center;
            block.HorizontalAlignment = HorizontalAlignment.Center;
            block.Text = $"Краткое описание блога";
            BlogMenu.FormAddOrChange.Children.Add(block);

            var descriptionBlog = new TextBox();
            descriptionBlog = new TextBox();
            descriptionBlog.VerticalAlignment = VerticalAlignment.Center;
            descriptionBlog.HorizontalAlignment = HorizontalAlignment.Center;
            BlogMenu.FormAddOrChange.Children.Add(descriptionBlog);
            descriptionBlog.Width = 310;

            block = new TextBlock();
            block.VerticalAlignment = VerticalAlignment.Center;
            block.HorizontalAlignment = HorizontalAlignment.Center;
            block.Text = $"Текст блога";
            BlogMenu.FormAddOrChange.Children.Add(block);

            var text = new TextBox();
            text = new TextBox();
            text.VerticalAlignment = VerticalAlignment.Center;
            text.HorizontalAlignment = HorizontalAlignment.Center;
            BlogMenu.FormAddOrChange.Children.Add(text);
            text.Width = 310;

            block = new TextBlock();
            block.VerticalAlignment = VerticalAlignment.Center;
            block.HorizontalAlignment = HorizontalAlignment.Center;
            block.Text = $"Картинка";
            BlogMenu.FormAddOrChange.Children.Add(block);

            block = new TextBlock();
            block.VerticalAlignment = VerticalAlignment.Center;
            block.HorizontalAlignment = HorizontalAlignment.Center;

            if (!needAdd)
            {
                nameBlog.Text = blog.Name;
                descriptionBlog.Text = blog.Description;
                text.Text = blog.Text;
                block.Text = blog.PathToImage;
            }

            var button = new Button();
            button.Content = "Добавить картинку";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            button.Click += (sender, e) =>
            {
                openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg";

                if (openFileDialog.ShowDialog() == true)
                    block.Text = openFileDialog.FileName;
            };
            BlogMenu.FormAddOrChange.Children.Add(button);
            BlogMenu.FormAddOrChange.Children.Add(block);

            button = new Button();
            button.Content = "Сохранить";

            button.Click += async (sender, e) =>
            {
                string path = block.Text;
                if (needAdd)
                {
                    string name = await Worker.UploadFile(path);
                    string newName = name.Replace("\"", "");
                    var newBlog = new BlogPage() { Name = nameBlog.Text, Description = descriptionBlog.Text, PathToImage = newName, Date = DateTime.Now, Text = text.Text };
                    await Worker.CreateBlog(newBlog);
                    MessageBox.Show($"Успешно сохранен новый блог", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    string newName = blog.PathToImage;
                    if (blog.PathToImage != path)
                    {
                        string name = await Worker.UploadFile(path);
                         newName = name.Replace("\"", "");
                    }
                    var newBlog = new BlogPage()
                    {
                        BlogId = blog.BlogId,
                        Name = nameBlog.Text,
                        Description = descriptionBlog.Text,
                        PathToImage = newName,
                        Date = DateTime.Now,
                        Text = text.Text
                    };
                    await Worker.UpdateBlog(newBlog);
                    MessageBox.Show($"Успешно изменен блог №{blog.BlogId}", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                BlogMenu.FormAddOrChange.Children.Clear();
                allGrid.Children.Remove(BlogMenu.FormAddOrChange);
                foreach (var item in BlogMenu.ElementsForDelete)
                {
                    allGrid.Children.Remove(item);
                }
                AddDataInAllDataBlog();
            };

            BlogMenu.FormAddOrChange.Children.Add(button);

        }
        #endregion

        #region Service
        private void Button_ClickService(object sender, RoutedEventArgs e)
        {
            CrateAndAddDataAllElementsService();
        }

        private void CrateAndAddDataAllElementsService()
        {
            if (NowWindowIsService)
            {
                return;
            }

            NowWindowIsService = true;

            ClearAllDataWorkTable();
            ClearAllDataContact();
            ClearAllDataBlog();
            ClearAllDataMain();
            ClearAllDataProject();

            AddDataInAllDataService();
        }

        private async void AddDataInAllDataService()
        {
            var data = await Worker.GetDataAllService();
            ServiceMenu.AllDataService.Items.Clear();
            if (!allGrid.Children.Contains(ServiceMenu.AllDataService))
            {
                Grid.SetRow(ServiceMenu.AllDataService, 1);
                Grid.SetColumn(ServiceMenu.AllDataService, 3);
                Grid.SetColumnSpan(ServiceMenu.AllDataService, 5);
                ServiceMenu.AllDataService.Margin = new Thickness(0, 0, 10, 0);
                foreach (var item in data)
                {
                    ServiceMenu.AllDataService.Items.Add($"{item.ServiceId}. {item.Name}");
                }
                allGrid.Children.Add(ServiceMenu.AllDataService);
                ServiceMenu.ElementsService.Add(ServiceMenu.AllDataService);
                ServiceMenu.AllDataService.SelectionChanged += (sender, e) =>
                {
                    var selectedService = (sender as ComboBox).SelectedItem as string;
                    ShowDataService(selectedService);
                };
            }
            else
            {
                ServiceMenu.AllDataService.Items.Clear();
                foreach (var item in data)
                {
                    ServiceMenu.AllDataService.Items.Add($"{item.ServiceId}. {item.Name}");
                }
            }

            var button = new Button();
            button.Content = "Добавить новую услугу";
            Grid.SetRow(button, 1);
            Grid.SetColumn(button, 8);
            Grid.SetColumnSpan(button , 2);
            button.Click += (sender, e) =>
            {
                CreateStackPanelForAddService();
            };
            allGrid.Children.Add(button);
            ServiceMenu.ElementsService.Add(button);

        }

        private void CreateStackPanelForAddService()
        {
            if (!allGrid.Children.Contains(ServiceMenu.FormAdd))
            {
                Grid.SetRow(ServiceMenu.FormAdd, 1);
                Grid.SetColumn(ServiceMenu.FormAdd, 11);
                Grid.SetColumnSpan(ServiceMenu.FormAdd, 5);
                Grid.SetRowSpan(ServiceMenu.FormAdd, 4);
                AddItemInStackPanelForService();
                allGrid.Children.Add(ServiceMenu.FormAdd);
                ServiceMenu.ElementsService.Add(ServiceMenu.FormAdd);
            }
            else
            {
                ServiceMenu.FormAdd.Children.Clear();
                AddItemInStackPanelForService();
            }
        }

        private void AddItemInStackPanelForService()
        {
            var block = new TextBlock();
            block.VerticalAlignment = VerticalAlignment.Center;
            block.HorizontalAlignment = HorizontalAlignment.Center;
            block.Text = "Добавление сервиса";
            block.Margin = new Thickness(0, 0, 0, 20);
            ServiceMenu.FormAdd.Children.Add(block);

            block = new TextBlock();
            block.VerticalAlignment = VerticalAlignment.Center;
            block.HorizontalAlignment = HorizontalAlignment.Center;
            block.Text = $"Название услуги";
            ServiceMenu.FormAdd.Children.Add(block);

            var nameService = new TextBox();
            nameService.VerticalAlignment = VerticalAlignment.Center;
            nameService.HorizontalAlignment = HorizontalAlignment.Center;
            ServiceMenu.FormAdd.Children.Add(nameService);
            nameService.Width = 310;

            block = new TextBlock();
            block.VerticalAlignment = VerticalAlignment.Center;
            block.HorizontalAlignment = HorizontalAlignment.Center;
            block.Text = $"Описание услуги";
            ServiceMenu.FormAdd.Children.Add(block);

            var textService = new TextBox();
            textService.VerticalAlignment = VerticalAlignment.Center;
            textService.HorizontalAlignment = HorizontalAlignment.Center;
            ServiceMenu.FormAdd.Children.Add(textService);
            textService.Width = 310;

            var button = new Button();
            button.Content = "Сохранить";

            button.Click += async (sender, e) =>
            {
                var service = new ServicePage() { Name = nameService.Text, Description = textService.Text};
                await Worker.CreateService(service);
                MessageBox.Show($"Успешно сохранена новая услуга", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                ServiceMenu.FormAdd.Children.Clear();
                allGrid.Children.Remove(ServiceMenu.FormAdd);
                foreach (var item in ServiceMenu.ElementsForDelete)
                {
                    allGrid.Children.Remove(item);
                }
                AddDataInAllDataService();
            };
            ServiceMenu.ElementsService.Add(button);
            ServiceMenu.FormAdd.Children.Add(button);
        }

        private async void ShowDataService(string selectedService)
        {
            if (selectedService is null)
            {
                return;
            }
            string id = selectedService.Split('.').First();
            var service = await Worker.GetConcreteService(id);

            if (!allGrid.Children.Contains(ServiceMenu.BlockName))
            {
                ServiceMenu.BlockName.VerticalAlignment = VerticalAlignment.Top;
                ServiceMenu.BlockName.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetRow(ServiceMenu.BlockName, 3);
                Grid.SetColumn(ServiceMenu.BlockName, 3);
                Grid.SetColumnSpan(ServiceMenu.BlockName, 3);
                ServiceMenu.BlockName.Text = $"Название услуги";
                ServiceMenu.BlockName.Margin = new Thickness(0, 0, 10, 10);
                ServiceMenu.ElementsService.Add(ServiceMenu.BlockName);
                ServiceMenu.ElementsForDelete.Add(ServiceMenu.BlockName);
                allGrid.Children.Add(ServiceMenu.BlockName);
            }

            if (!allGrid.Children.Contains(ServiceMenu.NewBoxName))
            {
                ServiceMenu.NewBoxName.VerticalAlignment = VerticalAlignment.Top;
                ServiceMenu.NewBoxName.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetRow(ServiceMenu.NewBoxName, 3);
                Grid.SetColumn(ServiceMenu.NewBoxName, 6);
                Grid.SetColumnSpan(ServiceMenu.NewBoxName, 2);
                ServiceMenu.NewBoxName.Width = 120;
                ServiceMenu.ElementsService.Add(ServiceMenu.NewBoxName);
                ServiceMenu.ElementsForDelete.Add(ServiceMenu.NewBoxName);
                allGrid.Children.Add(ServiceMenu.NewBoxName);
            }
            ServiceMenu.NewBoxName.Text = service.Name;

            if (!allGrid.Children.Contains(ServiceMenu.BlockText))
            {
                ServiceMenu.BlockText.VerticalAlignment = VerticalAlignment.Top;
                ServiceMenu.BlockText.HorizontalAlignment = HorizontalAlignment.Left;
                ServiceMenu.BlockText.Text = $"Текст услуги";
                Grid.SetRow(ServiceMenu.BlockText, 4);
                Grid.SetColumn(ServiceMenu.BlockText, 3);
                Grid.SetColumnSpan(ServiceMenu.BlockText, 3);
                ServiceMenu.BlockText.Margin = new Thickness(0, 0, 10, 10);
                ServiceMenu.ElementsService.Add(ServiceMenu.BlockText);
                ServiceMenu.ElementsForDelete.Add(ServiceMenu.BlockText);
                allGrid.Children.Add(ServiceMenu.BlockText);
            }

            if (!allGrid.Children.Contains(ServiceMenu.NewBoxText))
            {
                ServiceMenu.NewBoxText.VerticalAlignment = VerticalAlignment.Top;
                ServiceMenu.NewBoxText.HorizontalAlignment = HorizontalAlignment.Left;
                ServiceMenu.NewBoxText.TextWrapping = TextWrapping.Wrap;
                Grid.SetRow(ServiceMenu.NewBoxText, 4);
                Grid.SetColumn(ServiceMenu.NewBoxText, 6);
                Grid.SetColumnSpan(ServiceMenu.NewBoxText, 4);
                Grid.SetRowSpan(ServiceMenu.NewBoxText, 4);
                ServiceMenu.NewBoxText.Width = 120;
                ServiceMenu.ElementsService.Add(ServiceMenu.NewBoxText);
                ServiceMenu.ElementsForDelete.Add(ServiceMenu.NewBoxText);
                allGrid.Children.Add(ServiceMenu.NewBoxText);
            }
            ServiceMenu.NewBoxText.Text = service.Description;

            var button = new Button();
            button.Content = "Изменить";
            Grid.SetRow(button, 10);
            Grid.SetColumn(button, 3);
            Grid.SetColumnSpan(button, 2);
            button.Click += async (sender, e) =>
            {
                if (service.Name == ServiceMenu.NewBoxName.Text && service.Description == ServiceMenu.NewBoxText.Text)
                {
                    MessageBox.Show($"Вы не изменили никакие поля!", "Eror", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var newService = new ServicePage()
                {
                    ServiceId = service.ServiceId,
                    Name = ServiceMenu.NewBoxName.Text,
                    Description = ServiceMenu.NewBoxText.Text
                };
                await Worker.UpdateService(newService);
                MessageBox.Show($"Изменена услуга", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

                foreach (var item in ServiceMenu.ElementsForDelete)
                {
                    allGrid.Children.Remove(item);
                }
                AddDataInAllDataService();
            };
            allGrid.Children.Add(button);
            ServiceMenu.ElementsService.Add(button);
            ServiceMenu.ElementsForDelete.Add(button);

            button = new Button();
            button.Content = "Удалить";
            Grid.SetRow(button, 10);
            Grid.SetColumn(button, 6);
            Grid.SetColumnSpan(button, 2);
            button.Click += async (sender, e) =>
            {
                await Worker.DeleteService(service.ServiceId);
                MessageBox.Show($"Удалена услуга", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

                foreach (var item in ServiceMenu.ElementsForDelete)
                {
                    allGrid.Children.Remove(item);
                }
                AddDataInAllDataService();
            };
            allGrid.Children.Add(button);
            ServiceMenu.ElementsService.Add(button);
            ServiceMenu.ElementsForDelete.Add(button);
        }
        #endregion

        #region Contact
        private void Button_Contact(object sender, RoutedEventArgs e)
        {
            CrateAndAddDataAllElementsContact();
        }

        private async void CrateAndAddDataAllElementsContact()
        {
            if (NowWindowIsContact)
            {
                return;
            }

            NowWindowIsContact = true;

            ClearAllDataWorkTable();
            ClearAllDataProject();
            ClearAllDataBlog();
            ClearAllDataMain();
            ClearAllDataService();
            AddDataInAllDataContact();

            var address = new TextBlock();
            Grid.SetColumn(address, 11);
            Grid.SetRow(address, 8);
            Grid.SetColumnSpan(address, 3);
            address.VerticalAlignment = VerticalAlignment.Center;
            address.HorizontalAlignment = HorizontalAlignment.Center;
            address.Text = $"Адрес:";
            address.Margin = new Thickness(0, 0, 10, 0);
            allGrid.Children.Add(address);
            ContactMenu.ElementsContact.Add(address);

            var contactPage = await Worker.GetContact();

            var newAddres = new TextBox();
            Grid.SetColumn(newAddres, 14);
            Grid.SetRow(newAddres, 8);
            Grid.SetColumnSpan(newAddres, 3);
            newAddres.VerticalAlignment = VerticalAlignment.Center;
            newAddres.HorizontalAlignment = HorizontalAlignment.Center;
            newAddres.Text = contactPage.Address;
            newAddres.Width = 150;
            newAddres.Height = 20;
            allGrid.Children.Add(newAddres);
            ContactMenu.ElementsContact.Add(newAddres);

            var phone = new TextBlock();
            Grid.SetColumn(phone, 11);
            Grid.SetRow(phone, 9);
            Grid.SetColumnSpan(phone, 3);
            phone.VerticalAlignment = VerticalAlignment.Center;
            phone.HorizontalAlignment = HorizontalAlignment.Center;
            phone.Text = $"Номер:";
            phone.Margin = new Thickness(0, 0, 10, 0);
            allGrid.Children.Add(phone);
            ContactMenu.ElementsContact.Add(phone);

            var newPhone = new TextBox();
            Grid.SetColumn(newPhone,14);
            Grid.SetRow(newPhone, 9);
            Grid.SetColumnSpan(newPhone, 3);
            newPhone.VerticalAlignment = VerticalAlignment.Center;
            newPhone.HorizontalAlignment = HorizontalAlignment.Center;
            newPhone.Text = contactPage.Phone;
            newPhone.Width = 150;
            newPhone.Height = 20;
            allGrid.Children.Add(newPhone);
            ContactMenu.ElementsContact.Add(newPhone);

            var email = new TextBlock();
            Grid.SetColumn(email, 11);
            Grid.SetRow(email, 10);
            Grid.SetColumnSpan(email, 3);
            email.VerticalAlignment = VerticalAlignment.Center;
            email.HorizontalAlignment = HorizontalAlignment.Center;
            email.Text = $"Email:";
            email.Margin = new Thickness(0, 0, 10, 0);
            allGrid.Children.Add(email);
            ContactMenu.ElementsContact.Add(email);

            var newEmail = new TextBox();
            Grid.SetColumn(newEmail, 14);
            Grid.SetRow(newEmail, 10);
            Grid.SetColumnSpan(newEmail, 3);
            newEmail.VerticalAlignment = VerticalAlignment.Center;
            newEmail.HorizontalAlignment = HorizontalAlignment.Center;
            newEmail.Text = contactPage.Email;
            newEmail.Width = 150;
            newEmail.Height = 20;
            allGrid.Children.Add(newEmail);
            ContactMenu.ElementsContact.Add(newEmail);

            var button = new Button();
            button.Content = "Сохранить";
            Grid.SetRow(button, 11);
            Grid.SetColumn(button, 13);
            Grid.SetColumnSpan(button, 2);
            button.VerticalAlignment = VerticalAlignment.Center;
            button.HorizontalAlignment = HorizontalAlignment.Center;
            button.Margin = new Thickness(0, 0, 10, 0);
            button.Click += (sender, e) =>
            {
                var newContactPage = new ContactPage()
                {
                    Address = newAddres.Text,
                    Phone = newPhone.Text,
                    Email = newEmail.Text
                };
                if (contactPage.Address == newContactPage.Address && contactPage.Phone == newContactPage.Phone && contactPage.Email == newContactPage.Email)
                {
                    MessageBox.Show($"Вы не изменили никаких данных!", "INFO", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Worker.UpdateContact(newContactPage);
                    MessageBox.Show($"Успешно сохранены новые данные на странице контакты!", "Измененены данные", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            };
            allGrid.Children.Add(button);
            ContactMenu.ElementsContact.Add(button);
        }

        private async void AddDataInAllDataContact()
        {
            var data = await Worker.GetDataSocialNetwork();
            ContactMenu.AllDataContact.Items.Clear();
            if (!allGrid.Children.Contains(ContactMenu.AllDataContact))
            {
                Grid.SetRow(ContactMenu.AllDataContact, 1);
                Grid.SetColumn(ContactMenu.AllDataContact, 3);
                Grid.SetColumnSpan(ContactMenu.AllDataContact, 5);
                ContactMenu.AllDataContact.Margin = new Thickness(0, 0, 10, 0);
                foreach (var item in data)
                {
                    ContactMenu.AllDataContact.Items.Add($"{item.SocialNetworkId}. {item.PathToSite}");
                }
                allGrid.Children.Add(ContactMenu.AllDataContact);
                ContactMenu.ElementsContact.Add(ContactMenu.AllDataContact);
                ContactMenu.AllDataContact.SelectionChanged += (sender, e) =>
                {
                    var selectedNetWork = (sender as ComboBox).SelectedItem as string;
                    ShowDataContact(selectedNetWork);
                };
            }
            else
            {
                foreach (var item in data)
                {
                    ContactMenu.AllDataContact.Margin = new Thickness(0, 0, 10, 0);
                    ContactMenu.AllDataContact.Items.Add($"{item.SocialNetworkId}. {item.PathToSite}");
                }
            }

            var button = new Button();
            button.Content = "Добавить";
            Grid.SetRow(button, 1);
            Grid.SetColumn(button, 10);
            Grid.SetColumnSpan(button, 2);
            button.Click += (sender, e) =>
            {
                CreateStackPanelForAddOrChangeContact(true);
            };
            allGrid.Children.Add(button);
            ContactMenu.ElementsContact.Add(button);
            button = new Button();
            button.Content = "Изменить";
            Grid.SetRow(button, 2);
            Grid.SetColumn(button, 10);
            Grid.SetColumnSpan(button, 2);
            button.Click += (sender, e) =>
            {
                if (ContactMenu.AllDataContact.SelectedItem == null)
                {
                    MessageBox.Show($"Не выбрана соц сеть которую надо изменить", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                CreateStackPanelForAddOrChangeContact(false);
            };
            button.Margin = new Thickness(0, 10, 0, 0);
            allGrid.Children.Add(button);
            ContactMenu.ElementsContact.Add(button);

            button = new Button();
            button.Content = "Удалить";
            Grid.SetRow(button, 3);
            Grid.SetColumn(button, 10);
            Grid.SetColumnSpan(button, 2);
            button.Click += DeleteContact;
            button.Margin = new Thickness(0, 10, 0, 0);
            allGrid.Children.Add(button);
            ContactMenu.ElementsContact.Add(button);
        }

        private async void DeleteContact(object sender, RoutedEventArgs e)
        {
            if (ContactMenu.AllDataContact.SelectedItem != null)
            {
                var nameContact = ContactMenu.AllDataContact.SelectedItem.ToString();
                var id = nameContact.Split('.').FirstOrDefault();
                await Worker.DeleteSocialNetwork(Convert.ToInt32(id));
                AddDataInAllDataContact();
                MessageBox.Show("Успешно удалено!", "Ok", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Не выбрана соц сеть которую надо удалить!", "Eror", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ContactMenu.FormAddOrChange.Children.Clear();
            allGrid.Children.Remove(ContactMenu.FormAddOrChange);
            foreach (var item in ContactMenu.ElementsForDelete)
            {
                allGrid.Children.Remove(item);
            }
            AddDataInAllDataContact();
        }

        private async void ShowDataContact(string selectedNetWork)
        {
            if (selectedNetWork is null)
            {
                return;
            }
            string id = selectedNetWork.Split('.').First();
            var network = await Worker.GetConcreteSocialNetwork(id);

            if (!allGrid.Children.Contains(ContactMenu.BlockName))
            {
                ContactMenu.BlockName.VerticalAlignment = VerticalAlignment.Top;
                ContactMenu.BlockName.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetRow(ContactMenu.BlockName, 3);
                Grid.SetColumn(ContactMenu.BlockName, 3);
                Grid.SetColumnSpan(ContactMenu.BlockName, 3);
                ContactMenu.BlockName.Text = $"Ссылка на соц сеть";
                ContactMenu.BlockName.Margin = new Thickness(0, 0, 10, 10);
                ContactMenu.ElementsContact.Add(ContactMenu.BlockName);
                ContactMenu.ElementsForDelete.Add(ContactMenu.BlockName);
                allGrid.Children.Add(ContactMenu.BlockName);
            }

            if (!allGrid.Children.Contains(ContactMenu.NewBlockName))
            {
                ContactMenu.NewBlockName.VerticalAlignment = VerticalAlignment.Top;
                ContactMenu.NewBlockName.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetRow(ContactMenu.NewBlockName, 3);
                Grid.SetColumn(ContactMenu.NewBlockName, 6);
                Grid.SetColumnSpan(ContactMenu.NewBlockName, 2);
                ContactMenu.ElementsContact.Add(ContactMenu.NewBlockName);
                ContactMenu.ElementsForDelete.Add(ContactMenu.NewBlockName);
                allGrid.Children.Add(ContactMenu.NewBlockName);
            }
            ContactMenu.NewBlockName.Text = network.PathToSite;

            if (!allGrid.Children.Contains(ContactMenu.BlockImage))
            {
                ContactMenu.BlockImage.VerticalAlignment = VerticalAlignment.Top;
                ContactMenu.BlockImage.HorizontalAlignment = HorizontalAlignment.Left;
                ContactMenu.BlockImage.Text = $"Картинка";
                Grid.SetRow(ContactMenu.BlockImage, 4);
                Grid.SetColumn(ContactMenu.BlockImage, 3);
                Grid.SetColumnSpan(ContactMenu.BlockImage, 3);
                ContactMenu.ElementsContact.Add(ContactMenu.BlockImage);
                ContactMenu.ElementsForDelete.Add(ContactMenu.BlockImage);
                allGrid.Children.Add(ContactMenu.BlockImage);
            }


            string fullPath = await Worker.GetImage(network.PathToImage);
            if (File.Exists(fullPath))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(fullPath, UriKind.Absolute);
                bitmap.EndInit();
                if (allGrid.Children.Contains(ContactMenu.NewBlockImage))
                {
                    allGrid.Children.Remove(ContactMenu.NewBlockImage);
                }
                if (!allGrid.Children.Contains(ContactMenu.MyImage))
                {
                    Grid.SetRow(ContactMenu.MyImage, 4);
                    Grid.SetColumn(ContactMenu.MyImage, 6);
                    Grid.SetColumnSpan(ContactMenu.MyImage, 4);
                    Grid.SetRowSpan(ContactMenu.MyImage, 4);
                    ContactMenu.ElementsContact.Add(ContactMenu.MyImage);
                    ContactMenu.ElementsForDelete.Add(ContactMenu.MyImage);
                    allGrid.Children.Add(ContactMenu.MyImage);
                }
                ContactMenu.MyImage.Source = bitmap;
            }
            else
            {
                if (allGrid.Children.Contains(ContactMenu.MyImage))
                {
                    allGrid.Children.Remove(ContactMenu.MyImage);
                }
                if (!allGrid.Children.Contains(ContactMenu.NewBlockImage))
                {
                    ContactMenu.NewBlockImage.Text = "Картинки не существует";
                    Grid.SetRow(ContactMenu.NewBlockImage, 4);
                    Grid.SetColumn(ContactMenu.NewBlockImage, 6);
                    Grid.SetColumnSpan(ContactMenu.NewBlockImage, 4);
                    ContactMenu.ElementsContact.Add(ContactMenu.NewBlockImage);
                    ContactMenu.ElementsForDelete.Add(ContactMenu.NewBlockImage);
                    allGrid.Children.Add(ContactMenu.NewBlockImage);
                }
            }
        }

        private void CreateStackPanelForAddOrChangeContact(bool needAdd)
        {
            if (!allGrid.Children.Contains(ContactMenu.FormAddOrChange))
            {
                Grid.SetRow(ContactMenu.FormAddOrChange, 1);
                Grid.SetColumn(ContactMenu.FormAddOrChange, 13);
                Grid.SetColumnSpan(ContactMenu.FormAddOrChange, 5);
                Grid.SetRowSpan(ContactMenu.FormAddOrChange, 7);
                AddItemInStackPanelContact(needAdd);
                allGrid.Children.Add(ContactMenu.FormAddOrChange);
                ContactMenu.ElementsContact.Add(ContactMenu.FormAddOrChange);
            }
            else
            {
                ContactMenu.FormAddOrChange.Children.Clear();
                AddItemInStackPanelContact(needAdd);
            }
        }

        private async void AddItemInStackPanelContact(bool needAdd)
        {
            var network = new SocialNetwork();
            if (!needAdd)
            {
                var id = ContactMenu.AllDataContact.SelectedItem.ToString().Split('.').First();
                network = await Worker.GetConcreteSocialNetwork(id);
            }
            var block = new TextBlock();
            block.VerticalAlignment = VerticalAlignment.Center;
            block.HorizontalAlignment = HorizontalAlignment.Center;
            block.Text = needAdd ? $"Добавление соц сети" : $"Изменение соц сети";
            ContactMenu.FormAddOrChange.Children.Add(block);

            block = new TextBlock();
            block.VerticalAlignment = VerticalAlignment.Center;
            block.HorizontalAlignment = HorizontalAlignment.Center;
            block.Text = $"Ссылка на соц сеть";
            ContactMenu.FormAddOrChange.Children.Add(block);

            var linkNetwork = new TextBox();
            linkNetwork.VerticalAlignment = VerticalAlignment.Center;
            linkNetwork.HorizontalAlignment = HorizontalAlignment.Center;
            ContactMenu.FormAddOrChange.Children.Add(linkNetwork);
            linkNetwork.Width = 310;

            block = new TextBlock();
            block.VerticalAlignment = VerticalAlignment.Center;
            block.HorizontalAlignment = HorizontalAlignment.Center;
            block.Text = $"Картинка";
            ContactMenu.FormAddOrChange.Children.Add(block);

            block = new TextBlock();
            block.VerticalAlignment = VerticalAlignment.Center;
            block.HorizontalAlignment = HorizontalAlignment.Center;

            if (!needAdd)
            {
                linkNetwork.Text = network.PathToSite;
                block.Text = network.PathToImage;
            }

            var button = new Button();
            button.Content = "Добавить картинку";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            button.Click += (sender, e) =>
            {
                openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg";

                if (openFileDialog.ShowDialog() == true)
                    block.Text = openFileDialog.FileName;
            };
            ContactMenu.FormAddOrChange.Children.Add(button);
            ContactMenu.FormAddOrChange.Children.Add(block);

            button = new Button();
            button.Content = "Сохранить";

            button.Click += async (sender, e) =>
            {
                string path = block.Text;
                if (string.IsNullOrEmpty(path))
                {
                    MessageBox.Show($"Вы не добавили картинку", "Eror", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (needAdd)
                {
                    string name = await Worker.UploadFile(path);
                    string newName = name.Replace("\"", "");
                    var newNetwork = new SocialNetwork() { PathToSite = linkNetwork.Text, PathToImage = newName };
                    await Worker.CreateNetwork(newNetwork);
                    MessageBox.Show($"Успешно сохранен новая соц сеть", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    string newName = network.PathToImage;
                    if (network.PathToImage != path)
                    {
                        string name = await Worker.UploadFile(path);
                         newName = name.Replace("\"", "");
                    }
                    var newNetwork = new SocialNetwork() { SocialNetworkId = network.SocialNetworkId, PathToSite = linkNetwork.Text, PathToImage = newName };
                    await Worker.UpdateSocialNetwork(newNetwork);
                    MessageBox.Show($"Успешно изменена соц сеть №{network.SocialNetworkId}", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                ContactMenu.FormAddOrChange.Children.Clear();
                allGrid.Children.Remove(ContactMenu.FormAddOrChange);
                foreach (var item in ContactMenu.ElementsForDelete)
                {
                    allGrid.Children.Remove(item);
                }
                AddDataInAllDataContact();
            };

            ContactMenu.FormAddOrChange.Children.Add(button);
        }
        #endregion

        #region ClearAll
        private void ClearAllDataMain()
        {
            foreach (var item in MainMenu.ElementsMain)
            {
                allGrid.Children.Remove(item);
            }
            MainMenu.ElementsMain.Clear();
            NowWindowIsMain = false;
        }
        private void ClearAllDataService()
        {
            foreach (var item in ServiceMenu.ElementsService)
            {
                allGrid.Children.Remove(item);
            }
            NowWindowIsService = false;
            ServiceMenu.ElementsService.Clear();
            ServiceMenu.FormAdd.Children.Clear();
        }

        private void ClearAllDataProject()
        {
            foreach (var item in ProjectMenu.ElementsProject)
            {
                allGrid.Children.Remove(item);
            }
            foreach (var item in ProjectMenu.ElementsForDelete)
            {
                allGrid.Children.Remove(item);
            }
            NowWindowIsProject = false;
            ProjectMenu.ElementsProject.Clear();
            ProjectMenu.FormAddOrChange.Children.Clear();
        }

        private void ClearAllDataBlog()
        {
            foreach (var item in BlogMenu.ElementsBlog)
            {
                allGrid.Children.Remove(item);
            }
            foreach (var item in BlogMenu.ElementsForDelete)
            {
                allGrid.Children.Remove(item);
            }
            NowWindowIsBlog = false;
            BlogMenu.ElementsBlog.Clear();
            BlogMenu.FormAddOrChange.Children.Clear();
        }

        private void ClearAllDataContact()
        {
            foreach (var item in ContactMenu.ElementsContact)
            {
                allGrid.Children.Remove(item);
            }
            NowWindowIsContact = false;
            ContactMenu.ElementsForDelete.Clear();
            ContactMenu.FormAddOrChange.Children.Clear();
        }

        private void ClearAllDataWorkTable()
        {
            foreach (var item in WorkTableMenu.ElementsWorkTable)
            {
                allGrid.Children.Remove(item);
            }

            NowWindowIsWorkTable = false;

            WorkTableMenu.ElementsWorkTable.Clear();
            WorkTableMenu.AllRequest.Items.Clear();
        }
        #endregion
    }
}
