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

namespace UploadGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (UsernameText.Text == "a@b.c" && PasswordText.Text == "123")
            {
                UpLoadWin w1 = new UpLoadWin();
                Application.Current.MainWindow = w1;
                this.Close();
                w1.Show();
            }
            else
            {
                MessageBox.Show("Your Username or Password is invalid！");
            }

        }

        private void PasswordText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
