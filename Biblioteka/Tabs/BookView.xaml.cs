using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Biblioteka.HelpWindows;
using Biblioteka.Class;
namespace Biblioteka.Tabs
{
    /// <summary>
    /// Interaction logic for BookView.xaml
    /// </summary>
    public partial class BookView : UserControl
    {
        
        public ObservableCollection<string> Zanrovi { get; set; }
        public ObservableCollection<Class.Book> FiltriranaKolekcija { get; set; }

        public Class.Biblioteka b = new Class.Biblioteka();

        public bool Checked = false;
        public string zanr;
        public bool ComboCheck = false;

        public BookView()
        {
            InitializeComponent();
            b.Import();

            Zanrovi = new ObservableCollection<string>(b.biblioteka.Select(book => book.Zanr).Distinct());
            Zanrovi.Add("Unselect Genre");

            Tabela.ItemsSource = b.biblioteka;
            ZanrSearch.ItemsSource = Zanrovi;
            
            DataContext = this;

            FiltriranaKolekcija = new ObservableCollection<Class.Book>();
        }


        private void SearchBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Search")
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black;
            }
        }
        private void SearchBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Search";
                textBox.Foreground = Brushes.Gray;
            }
        }
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterData();
        }
        private void Dostupnost_Checked(object sender, RoutedEventArgs e)
        {
            Checked = true;
            FilterData();
        }
        private void ZanrSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                if (comboBox.SelectedItem != null && comboBox.SelectedItem.ToString() == "Unselect Genre")
                {
                    comboBox.SelectedItem = null;
                    
                    hintText.Visibility = Visibility.Visible;
                }

                ComboCheck = comboBox.SelectedItem != null;
                zanr = comboBox.SelectedItem?.ToString();
                FilterData();
            }
        }
        private void Dostupnost_Unchecked(object sender, RoutedEventArgs e)
        {
            Checked = false;
            FilterData();
        }
        private void ZanrSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ZanrSearch.SelectedIndex >= 0)
            {
                hintText.Visibility = Visibility.Collapsed;
            }
        }
        private void ZanrSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ZanrSearch.SelectedIndex <= 0)
            {
                hintText.Visibility = Visibility.Visible;
            }
        }
        private void FilterData()
        {
            var filteredList = b.biblioteka.ToList();

            // Filter by search text
            if (!string.IsNullOrEmpty(SearchBox.Text) && SearchBox.Text != "Search")
            {
                string searchText = new string(SearchBox.Text.Trim().Where(c => char.IsLetterOrDigit(c) || c == ' ').ToArray());
                filteredList = filteredList.Where(x => x.Naslov.ToLower().Contains(searchText.ToLower()) || x.Autor.ToLower().Contains(searchText.ToLower())).ToList();
            }

            // Filter by zanr
            if (ComboCheck == true)
            {
                filteredList = filteredList.Where(x => x.Zanr == zanr).ToList();
            }
            
            // Filter by dostupnost
            if (Checked == true)
            {
                filteredList = filteredList.Where(x => x.Dostupnost).ToList();
            }
            
            FiltriranaKolekcija = new ObservableCollection<Class.Book>(filteredList);
            Tabela.ItemsSource = FiltriranaKolekcija;
        }
        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            BookAdd prozor = new BookAdd(this);
            prozor.ShowDialog();
        }
        private void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Book clickedItem = (Book)button.DataContext;
            int index = b.biblioteka.IndexOf(clickedItem);
            
            
            
            BookEdit prozor = new BookEdit(this,b.biblioteka[index]);
            prozor.ShowDialog();
            
            Tabela.Items.Refresh();
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Book clickedItem = (Book)button.DataContext;
            int index = b.biblioteka.IndexOf(clickedItem);
            try {
                b.biblioteka.RemoveAt(index); 
            }catch(Exception ex)
            {
                var o = ex;
            }
            Tabela.Items.Refresh();
            b.Export();
            
        }

     


    }
}
