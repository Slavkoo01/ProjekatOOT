using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Win32;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Biblioteka.Class;
using Biblioteka.Tabs;
using System.Collections.ObjectModel;
namespace Biblioteka.HelpWindows
{
   
    public partial class UserAdd : Window
    {
        private BookRent bookRentInstance;       



        public UserAdd(BookRent bookRent)
        {
            InitializeComponent();
            bookRentInstance = bookRent;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SifraBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "ID")
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
                textBox.Text = "ID";
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void NaslovBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Ime")
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
                textBox.Text = "Ime";
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void AutorBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Prezime")
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
                textBox.Text = "Prezime";
                textBox.Foreground = Brushes.Gray;
            }
        }

        


       
       

        private void BookAdd_Button_Click(object sender, RoutedEventArgs e)
        {

            if (SifraBox.Text != "" && SifraBox.Text != "ID")
            {

                if (!bookRentInstance.k.IdPostoji(SifraBox.Text))
                {
                    


                    User user = new User(SifraBox.Text, ImeBox.Text == "Ime" ? "-Empty-" : ImeBox.Text, PrezimeBox.Text == "Prezime" ? "-Empty-" : PrezimeBox.Text);
                    bookRentInstance.k.Dodaj(user);
                    bookRentInstance.k.Append(user);
                    
                    bookRentInstance.UsersTree.Items.Refresh();
                    
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
