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
using System.Collections.ObjectModel;
using System.IO;
using Biblioteka.HelpWindows;

namespace Biblioteka.Tabs
{
    /// <summary>
    /// Interaction logic for BookRent.xaml
    /// </summary>
    public partial class BookRent : UserControl
    {
        private Class.Biblioteka _biblioteka;

        public ObservableCollection<string> Zanrovi { get; set; }
        public ObservableCollection<Class.Book> Biblioteka { get; set; }
        public ObservableCollection<Class.User> Korisnici { get; set; }
        public ObservableCollection<Class.BooksByGenres> KnjigePoZanrovima { get; set; }
        public BookRent()
        {
            InitializeComponent();
            InitializeData();
            DataContext = this;
        }
        private void InitializeData()
        {
            KnjigePoZanrovima = new ObservableCollection<Class.BooksByGenres>();
            _biblioteka = new Class.Biblioteka();
            _biblioteka.Import();

            Zanrovi = new ObservableCollection<string>(_biblioteka.biblioteka.Select(book => book.Zanr).Distinct());
            //Biblioteka = new ObservableCollection<Class.Book>() { new Class.Book("1", "Book1", "Author1", "Romance", true), new Class.Book("2", "Book2", "Author2", "Fantasy", true) };
            Biblioteka = new ObservableCollection<Class.Book>(_biblioteka.biblioteka);
            Korisnici = new ObservableCollection<Class.User>() { new Class.User("bane","bane"), new Class.User("xd","xd")};

            foreach (string zanr in Zanrovi)
            {
                //KnjigePoZanrovima.Add(new Class.BooksByGenres(zanr, Biblioteka.Where(b => b.Zanr.ToLower() == zanr.ToLower()).ToList()));
                TreeViewItem genreNode = new TreeViewItem();
                genreNode.Header = zanr;

                foreach (Class.Book book in Biblioteka.Where(b => b.Zanr == zanr))
                {
                    TreeViewItem bookNode = new TreeViewItem();
                    bookNode.Header = book;
                    bookNode.IsEnabled = book.Dostupnost;
                    genreNode.Items.Add(bookNode);
                }

                KnjigeTreeView.Items.Add(genreNode);
            }

            foreach (Class.User korisnik in Korisnici)
            {
                TreeViewItem userNode = new TreeViewItem();
                userNode.Header = korisnik;

                foreach (Class.Book book in korisnik.knjigeIznajmljene)
                {
                    TreeViewItem bookNode = new TreeViewItem();
                    bookNode.Header = book;
                    userNode.Items.Add(bookNode);
                }

                KorisniciTreeView.Items.Add(userNode);
            }
        }

        private void KnjigeTreeView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                TreeView treeView = (TreeView)sender;
                TreeViewItem treeViewItem = FindTreeViewItem(e.OriginalSource as DependencyObject);

                if (treeViewItem != null)
                {
                    Class.Book selectedBook = (Class.Book)treeViewItem.Header;

                    if (selectedBook.Dostupnost)
                    {
                        DataObject dragData = new DataObject(typeof(Class.Book), selectedBook);
                        DragDrop.DoDragDrop(treeViewItem, dragData, DragDropEffects.Move);
                    }
                }
            }
        }

        private void KorisniciTreeView_Drop(object sender, DragEventArgs e)
        {
            TreeView treeView = (TreeView)sender;
            TreeViewItem targetUserNode = FindTreeViewItem(e.OriginalSource as DependencyObject);

            if (targetUserNode != null)
            {
                Class.User targetKorisnik = (Class.User)targetUserNode.Header;
                Class.Book draggedBook = (Class.Book)e.Data.GetData(typeof(Class.Book));

                if (targetKorisnik.knjigeIznajmljene.Contains(draggedBook))
                    return;

                targetKorisnik.knjigeIznajmljene.Add(draggedBook);
                targetUserNode.Items.Add(new TreeViewItem() { Header = draggedBook });

                draggedBook.Dostupnost = false;
                //DisableBookInTreeView(KnjigeTreeView, draggedBook);
            }
        }
        private void DisableBookInTreeView(TreeView treeView, Class.Book book)
        {
            TreeViewItem bookNode = FindBookTreeViewItem(treeView, book);

            if (bookNode != null)
            {
                bookNode.IsEnabled = false;
            }
        }
        private TreeViewItem FindTreeViewItem(DependencyObject source)
        {
            if (source == null)
                return null;

            DependencyObject parent = VisualTreeHelper.GetParent(source);

            if (parent is TreeViewItem treeViewItem)
            {
                return treeViewItem;
            }
            else
            {
                return FindTreeViewItem(parent);
            }
        }
        private TreeViewItem FindBookTreeViewItem(TreeView treeView, Class.Book book)
        {
            foreach (TreeViewItem genreNode in treeView.Items)
            {
                foreach (TreeViewItem bookNode in genreNode.Items)
                {
                    if (bookNode.Header == book)
                        return bookNode;
                }
            }

            return null;
        }

        private void DodajKorisnika_Click(object sender, RoutedEventArgs e)
        {
            //Class.User korisnik = new Class.User("epic","epic");
            //Korisnici.Add(korisnik);

            //TreeViewItem userNode = new TreeViewItem();
            //userNode.Header = korisnik;

            //KorisniciTreeView.Items.Add(userNode);
            UserAdd prozor = new UserAdd();
            prozor.ShowDialog();
        }
    }
}
