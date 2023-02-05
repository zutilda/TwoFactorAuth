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

namespace TwoFactorAuth
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    ///   public partial class MainWindow : Window
    public partial class MainWindow : Window
    {
        private const string Login = "admin";
        private const string Password = "admin";
        private Authorization AuthCode;
        private DispatcherTimer Timer;
        private int time;
        private string CAPTCHA;
        private bool FlagCAPTCHA = true;

        public MainWindow()
        {
            InitializeComponent();

            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = new TimeSpan(0, 0, 1);
        }

        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            if (LogIn.Text != Login || PasswordIn.Password != Password)
            {
                MessageBox.Show("Неверные данные для входа");
                return;
            }

            Random random = new Random();
            int entryCode = random.Next(10000, 100000);
            MessageBox.Show(entryCode.ToString(), "Код");

            AuthCode = new Authorization(entryCode);

            if ((bool)AuthCode.ShowDialog())
            {
                MessageBox.Show("Успешно!");
                Auth.IsEnabled = true;
                LogIn.IsEnabled = true;
                PasswordIn.IsEnabled = true;
                NewCode.Visibility = Visibility.Collapsed;
            }
            else
            {
                Timer.Start();
                time = 10;
                Time.Text = "Получить новый код можно будет через " + time + " с";
                Auth.IsEnabled = false;
                LogIn.IsEnabled = false;
                PasswordIn.IsEnabled = false;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            time--;
            Time.Text = "Получить новый код можно будет через " + time + " с";

            if (time == 0)
            {
                Timer.Stop();
                NewCode.Visibility = Visibility.Visible;
                Time.Text = "";
            }
        }

        private void NewCode_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            int expectedCode = random.Next(10000, 100000);
            MessageBox.Show(expectedCode.ToString(), "Код");

            AuthCode = new Authorization(expectedCode);

            if ((bool)AuthCode.ShowDialog())
            {
                MessageBox.Show("Успешно!");
                Auth.IsEnabled = true;
                LogIn.IsEnabled = true;
                PasswordIn.IsEnabled = true;
                NewCode.Visibility = Visibility.Collapsed;
            }
            else
            {
                NewCode.Visibility = Visibility.Collapsed;
                CAPTCHA = GetCAPTHA();
                Canvas.Visibility = Visibility.Visible;
                Captha.Visibility = Visibility.Visible;
            }
        }

        private string GetCAPTHA()
        {
            Canvas.Children.Clear();
            Random rand = new Random();

            string result = "";
            char c = '0';
            int count = rand.Next(7, 11);

            for (int i = 0; i < count; i++)
            {
                switch (rand.Next(0, 3))
                {
                    case 0:
                        c = (char)rand.Next(49, 58);
                        break;
                    case 1:
                        c = (char)rand.Next(65, 91);
                        break;
                    case 2:
                        c = (char)rand.Next(97, 123);
                        break;
                }
                result += c;

                TextBlock tb = new TextBlock()
                {
                    Text = c.ToString(),
                    Padding = new Thickness(i * 25 + 5, rand.Next(21), rand.Next(21), 10),
                    FontSize = rand.Next(20, 26)
                };

                switch (rand.Next(0, 3))
                {
                    case 0:
                        tb.FontStyle = FontStyles.Italic;
                        break;
                    case 1:
                        tb.FontWeight = FontWeights.Bold;
                        break;
                    case 2:
                        tb.FontStyle = FontStyles.Italic;
                        tb.FontWeight = FontWeights.Bold;
                        break;
                }

                Canvas.Children.Add(tb);
            }

            for (int i = 0; i < 15; i++)
            {
                Line line = new Line()
                {
                    X1 = rand.Next(251),
                    Y1 = rand.Next(51),
                    X2 = rand.Next(251),
                    Y2 = rand.Next(51),
                    Stroke = new SolidColorBrush(Color.FromRgb((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256)))
                };
                Canvas.Children.Add(line);
            }

            return result;
        }

        private void Captha_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Captha.Text.Length == CAPTCHA.Length)
            {
                if (Captha.Text.ToLower() == CAPTCHA.ToLower())
                {
                    MessageBox.Show("Успешно!");
                    Auth.IsEnabled = true;
                    LogIn.IsEnabled = true;
                    PasswordIn.IsEnabled = true;
                    FlagCAPTCHA = true;
                    Captha.Visibility = Visibility.Collapsed;
                    Captha.Text = "";
                    Canvas.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (FlagCAPTCHA)
                    {
                        Captha.Text = "";
                        CAPTCHA = GetCAPTHA();
                        FlagCAPTCHA = false;
                    }
                    else
                    {
                        MessageBox.Show("Проверка не пройдена");
                        Auth.IsEnabled = true;
                        LogIn.IsEnabled = true;
                        PasswordIn.IsEnabled = true;
                        FlagCAPTCHA = true;
                        Captha.Visibility = Visibility.Collapsed;
                        Captha.Text = "";
                        Canvas.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
    }
}

