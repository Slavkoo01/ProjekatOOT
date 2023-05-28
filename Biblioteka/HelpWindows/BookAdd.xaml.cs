using Microsoft.Win32;
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
using System.IO;
using Biblioteka.Class;
using Biblioteka.Tabs;
using System.Collections.ObjectModel;

namespace Biblioteka.HelpWindows
{
    /// <summary>
    /// Interaction logic for BookAdd.xaml
    /// </summary>

    public partial class BookAdd : Window
    {
        private BookView bookViewInstance;
        private string ImageDestination;
        private string DestinationToCopy;
        private string ZanrBoxItem;
        private string ImagePath;

        public BookAdd(BookView bookView)
        {
            InitializeComponent();
            bookViewInstance = bookView;
        }
        
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ZanrBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var comboBox = sender as ComboBox;
            ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;

            if (selectedItem != null)
            {
                if (selectedItem.Content.ToString() == "Unselect Genre")
                {
                    comboBox.SelectedItem = null; 
                    hintText.Visibility = Visibility.Visible;
                    ZanrBoxItem = null; 
                }
                else
                {
                    ZanrBoxItem = selectedItem.Content.ToString();
                    hintText.Visibility = Visibility.Collapsed;
                }
            }

        }

        private void SifraBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Sifra")
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
                textBox.Text = "Sifra";
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void NaslovBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Naslov")
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
                textBox.Text = "Naslov";
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void AutorBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Autor")
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
                textBox.Text = "Autor";
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void ZanrBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ZanrBox.SelectedIndex <= 0)
            {
                hintText.Visibility = Visibility.Collapsed;
            }
        }

        private void ZanrBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ZanrBox.SelectedIndex <= 0)
            {
                hintText.Visibility = Visibility.Visible;
            }
        }
    

        private string PathImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png; *.jpg; *.jpeg)|*.png;*.jpg;*.jpeg";

            if (openFileDialog.ShowDialog() == true)
            {
                using (Stream fileStream = openFileDialog.OpenFile())
                {
                    ImageBox.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                    string fileName = openFileDialog.SafeFileName; 
                    string projectDirectory = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName).FullName;
                    string imagesDirectory = System.IO.Path.Combine(projectDirectory, "Images");
                    DestinationToCopy = System.IO.Path.Combine(imagesDirectory, fileName);
                    ImageDestination = openFileDialog.FileName;
                }

                return DestinationToCopy;
            }

            return null;
        }
        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {                          
            ImagePath = PathImage();
            
        }

        private void BookAdd_Button_Click(object sender, RoutedEventArgs e)
        {
            
            if (SifraBox.Text != "" && SifraBox.Text != "Sifra")
            {
                
                if (!bookViewInstance.b.SifraPostoji(SifraBox.Text))
                {
                        string path = @"\Images\";

                        if (ImageDestination != null)
                        {
                            try
                            {
                                File.Copy(ImageDestination, DestinationToCopy, true);
                            }
                            catch (IOException ex)
                            {
                                var o = ex;
                                
                            }
                        string[] ImagePathSplit = ImagePath.Split('\\');
                        path += ImagePathSplit[ImagePathSplit.Length-1];
                        }
                    
                    ImagePath = ImagePath == null ? @"\Images\defultBook.png" : path;
                    string projectDirectory = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName).FullName;
                    projectDirectory += ImagePath;


                    Book knjiga = new Book(SifraBox.Text, NaslovBox.Text == "Naslov" ? "-Empty-" : NaslovBox.Text, AutorBox.Text == "Autor" ? "-Empty-" : AutorBox.Text, ZanrBoxItem == null ? "-Empty-" : ZanrBoxItem, true, projectDirectory);
                    bookViewInstance.b.Dodaj(knjiga);
                    bookViewInstance.b.Append(knjiga);
                    bookViewInstance.Tabela.Items.Refresh();
                    UpdateZanrSearchBox();
                    bookViewInstance.b.Export();

                    Close();
                }
                else
                {
                    MessageBox.Show("Sifra vec postoji");
                }
            }
            else
            {
                MessageBox.Show("Morate Unjeti sifru");
            }
        }
        private void UpdateZanrSearchBox()
        {
            bookViewInstance.Zanrovi = new ObservableCollection<string>(bookViewInstance.b.biblioteka.Select(book => book.Zanr).Distinct());
            bookViewInstance.Zanrovi.Add("Unselect Genre");
            bookViewInstance.ZanrSearch.ItemsSource = bookViewInstance.Zanrovi;
            bookViewInstance.ZanrSearch.Items.Refresh();
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
