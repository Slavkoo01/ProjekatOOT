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

namespace Biblioteka.HelpWindows
{
    /// <summary>
    /// Interaction logic for UserAdd.xaml
    /// </summary>
    public partial class UserAdd : Window
    {
        public UserAdd()
        {
            InitializeComponent();
        }

        private void ImeTB_Focus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Ime")
            {
                textBox.Text = "";
            }
        }

        private void PrezimeTB_Focus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Prezime")
            {
                textBox.Text = "";
            }
        }

        private void ImeTB_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Ime";
            }
        }

        private void PrezimeTB_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Prezime";
            }
        }
    }
}
