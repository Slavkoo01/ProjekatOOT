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
using Biblioteka.Tabs;
using Biblioteka.Class;

namespace Biblioteka.HelpWindows
{
    
    public partial class UserEdit : Window
    {
        private BookRent bookRentInstance;
        private User user;


        public UserEdit(BookRent bookRent,User user)
        {
            InitializeComponent();
            bookRentInstance = bookRent;
            this.user = user;
            SifraBox.Text = user.Id;
            ImeBox.Text = user.Ime;
            PrezimeBox.Text = user.Prezime;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SifraBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Sifra" || textBox.Text == user.Id)
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black;
            }
        }

        private void SifraBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = user.Id;
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void NaslovBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Ime" || textBox.Text == user.Ime)
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black;
            }

        }

        private void NaslovBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = user.Ime;
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void AutorBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Prezime" || textBox.Text == user.Prezime)
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black;
            }


        }

        private void AutorBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = user.Prezime;
                textBox.Foreground = Brushes.Gray;
            }
        }

        





    private void BookEdit_Button_Click(object sender, RoutedEventArgs e)
        {
            bool SifraException = bookRentInstance.k.IdPostoji(SifraBox.Text);

            if (SifraBox.Text == user.Id)
            {
                SifraException = false;
            }



            if (SifraBox.Text != "" && SifraBox.Text != "ID")
            {

                if (!SifraException)
                {
                    
                    user.Id = SifraBox.Text;
                    user.Ime = ImeBox.Text == "Ime" ? "-Empty-" : ImeBox.Text;
                    user.Prezime = PrezimeBox.Text == "Prezime" ? "-Empty-" : PrezimeBox.Text;
                    


                    
                    try { bookRentInstance.UsersTree.Items.Refresh(); } catch (Exception) { }
                    bookRentInstance.k.Export();

                    Close();
                }
                else
                {
                    MessageBox.Show("Id vec postoji");
                }
            }
            else
            {
                MessageBox.Show("Morate Unjeti Id");
            }
        }

        private void WindowGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void WindowGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
