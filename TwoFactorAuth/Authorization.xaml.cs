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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TwoFactorAuth
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        int Code;
        private DispatcherTimer Timer;

        public Authorization(int code)
        {
            InitializeComponent();

            Code = code;
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = new TimeSpan(0, 0, 10);
            Timer.Start();
        }

        private void OneTimeCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (OneTimeCode.Text.Length == 5)
            {
                Timer.Stop();

                if (int.TryParse(OneTimeCode.Text, out int result))
                {
                    if (Code == result)
                    {
                        DialogResult = true;
                    }
                }
                else
                {
                    MessageBox.Show("Вы неверно ввели код!");
                    DialogResult = false;
                }

                Close();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Timer.Stop();
            DialogResult = false;
            Close();
        }
    }
}

