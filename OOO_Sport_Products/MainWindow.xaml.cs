using EasyCaptcha.Wpf;
using OOO_Sport_Products.Classes;
using OOO_Sport_Products.Model;
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
using System.Windows.Threading;

namespace OOO_Sport_Products
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String captchaText; 
        DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            
            const int lengthCaptcha = 4;            //Длина каптчи
            captcha.CreateCaptcha(Captcha.LetterOption.Alphanumeric, lengthCaptcha);
            captchaText = captcha.CaptchaText;	//Сформированная строка каптчи
            try
            {
                Helper.DB = new Model.DBSportProduct(); //связь с бд
                // MessageBox.Show("Связь с бд");
            }
            catch
            {
                MessageBox.Show("Проблема связи с базой");
                return;
            }
        }

        /// <summary>
        /// Завершение работы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Переход в каталог при авторизации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void buttonInput_Click(object sender, RoutedEventArgs e)
        {
            string Login = textboxLogin.Text;
            string Password = textboxpassword.Password;
            string tbcaptcha = textboxcaptcha.Text;
            StringBuilder sb = new StringBuilder();
            if(String.IsNullOrEmpty(Login))
                {
                sb.AppendLine("Вы забыли ввести логин");
            }
            if (String.IsNullOrEmpty(Password))
            {
                sb.AppendLine("Вы забыли ввести пароль");
            }
            if (String.IsNullOrEmpty(tbcaptcha))
            {
                sb.AppendLine("Вы забыли ввести капчу");
            }
            else if (captcha.CaptchaText != tbcaptcha)
            {
                sb.AppendLine("Капча неверная");
                buttonInput.IsEnabled = false;
                captcha.CreateCaptcha(Captcha.LetterOption.Alphanumeric, 4);
                captchaText = captcha.CaptchaText;  //Сохранение строки каптчи
                timer.Tick += new EventHandler(TimerTick);
                timer.Interval = new TimeSpan(40000000);
                timer.Start();
            }
            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString());
                return;
            }
            //Поиск в БД пользователя с таким логином и паролем
            //Получение списка пользователей в листе
            List<Model.User> users = new List<Model.User>();
            //users = Helper.DB.User.ToList();

            users = Helper.DB.User.Where(x => x.UserLogin == Login && x.UserPassword == Password).ToList();

            //users = Helper.DB.User.Where(x => x.UserLogin.Equals(Login).Equals(Password)).ToList();
            //Model.User user = users.FirstOrDefault();
            Helper.user = users.FirstOrDefault();
            sb.Clear();
            if (Helper.user != null)
            {
                sb.AppendLine(Helper.user.UserFullName);
                //sb.AppendLine(Helper.user.Role.RoleName);
                sb.AppendLine(Helper.DB.Role.Where(x => x.RoleID == Helper.user.Role.RoleID).Select(x => x.RoleName).FirstOrDefault());
                MessageBox.Show(sb.ToString());
                GoCatalog();
            }
            else
            {
                MessageBox.Show("Пусто");
            }


            //bool flag = false;
            //foreach (Model.User user in users)
            //{
            //    if (user.UserLogin == Login && user.UserPassword == Password) {
            //        MessageBox.Show("Есть");
            //    flag = true;
            //        break;
            //    }

            //}
            //if (!flag) {
            //    MessageBox.Show("НЕТ");
            //}

            //Model.User user = users.FirstOrDefault();
            //sb.Clear();
            //if(user != null) {
            //    sb.AppendLine(user.UserFullName);
            //    //sb.AppendLine(user.Role.RoleName);
            //    sb.AppendLine(Helper.DB.Role.Where(x => x.RoleID == user.Role.RoleID).Select(x => x.RoleName).FirstOrDefault());
            //    sb.AppendLine(user.UserLogin.ToString());
            //    sb.AppendLine(user.UserPassword.ToString());
            //    MessageBox.Show(sb.ToString());
            //}
            //else
            //{
            //    MessageBox.Show("Пусто");
            //}
        }


        private void TimerTick(object sender, EventArgs e)
        {
            buttonInput.IsEnabled = true;
            timer.Stop();
        }


        private void GoCatalog()
        {
            View.Catalog catalog = new View.Catalog();

            this.Hide();
            catalog.ShowDialog();
            this.Show();
        }

        private void buttonGuest_Click(object sender, RoutedEventArgs e)
        {
            Helper.user = null;
            GoCatalog();
        }

        private void isVisiblePassword_Checked(object sender, RoutedEventArgs e)
        {
            tbPassword.Text = textboxpassword.Password;
            tbPassword.Visibility = Visibility.Visible;
            textboxpassword.Visibility = Visibility.Hidden;
        }

        private void isVisiblePassword_Unchecked(object sender, RoutedEventArgs e)
        {
            textboxpassword.Password = tbPassword.Text;
            tbPassword.Visibility = Visibility.Hidden;
            textboxpassword.Visibility = Visibility.Visible;
        }
    }
}
