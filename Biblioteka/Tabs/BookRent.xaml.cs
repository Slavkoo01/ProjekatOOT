using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using Biblioteka.Class;
using Biblioteka.HelpWindows;

namespace Biblioteka.Tabs
{
    public partial class BookRent : UserControl
    {
        public Korisnici k = new Korisnici();
        public ObservableCollection<GenreNode> Genre;
        private Dictionary<object, bool> expandedBooksState;
        private Dictionary<object, bool> expandedUsersState;
        public BookView bookView = new BookView();
        public BookRent()
        {
            InitializeComponent();
            expandedBooksState = new Dictionary<object, bool>();
            expandedUsersState = new Dictionary<object, bool>();
            k.Import();
            

        }

        public void RefreshBookTree()
        {
            bookView = new BookView();
            Genre = new ObservableCollection<GenreNode>();

            foreach (var genre in bookView.Zanrovi)
            {
                if (genre != "Unselect Genre")
                {
                    var genreNode = new GenreNode(genre);
                    foreach (Book book in bookView.b.biblioteka)
                    {
                        if (book.Zanr == genre)
                            genreNode.Books.Add(book);
                    }
                    Genre.Add(genreNode);
                }
            }
            BooksTree.ItemsSource = Genre;
        }

        public void RefreshUserTree()
        {
            k = new Korisnici();
            k.Import();
            UsersTree.ItemsSource = k.korisnici;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshBookTree();
            RefreshUserTree();
        }

        private void Delete_User_Button_Click(object sender, RoutedEventArgs e)
        {
            StoreExpandedStateBooks();
            StoreExpandedStateUsers();
            Button button = (Button)sender;
            User clickedItem = (User)button.DataContext;
            int index = k.korisnici.IndexOf(clickedItem);

            try
            {
                foreach(Book book1 in k.korisnici[index].IznajmljeneKnjige)
                {
                    foreach (Book book in bookView.b.biblioteka)
                    {

                        if (book1.Sifra == book.Sifra)
                        {
                            book.Dostupnost = true;
                            break;
                        }
                    }
                }
                
                k.korisnici.RemoveAt(index);
                BooksTree.Items.Refresh();
            }
            catch (Exception)
            {
               
            }

            UsersTree.Items.Refresh();
            k.Export();
            bookView.b.Export();
            RestoreExpandedStateBooks();
            RestoreExpandedStateUsers();
            RemoveUserFromDataBase(clickedItem.Id);

        }


        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            StoreExpandedStateBooks();
            StoreExpandedStateUsers();
            UserAdd prozor = new UserAdd(this);
            prozor.ShowDialog();
            RestoreExpandedStateBooks();
            RestoreExpandedStateUsers();
        }

        

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            StoreExpandedStateBooks();
            StoreExpandedStateUsers();
            Button button = (Button)sender;
            Book clickedBook = (Book)button.DataContext;

            User user = k.korisnici.FirstOrDefault(u => u.IznajmljeneKnjige.Contains(clickedBook));

            if (user != null)
            {
                user.IznajmljeneKnjige.Remove(clickedBook);
                clickedBook.Dostupnost = true;
                
                foreach(Book book in bookView.b.biblioteka)
                {
                    
                    if(book.Sifra == clickedBook.Sifra)
                    {
                        book.Dostupnost = true;
                        break;
                    }
                }

                UsersTree.Items.Refresh();
                BooksTree.Items.Refresh();

                k.Export();
                bookView.b.Export();
            RestoreExpandedStateUsers();
            RestoreExpandedStateBooks();
            }
        }

        private void Edit_User_Button_Click(object sender, RoutedEventArgs e)
        {
            StoreExpandedStateBooks();
            StoreExpandedStateUsers();
            Button button = (Button)sender;
            User clickedItem = (User)button.DataContext;
            int index = k.korisnici.IndexOf(clickedItem);

            UserEdit prozor = new UserEdit(this, k.korisnici[index]);
            prozor.ShowDialog();

            UsersTree.Items.Refresh();
            RestoreExpandedStateUsers();
            RestoreExpandedStateBooks();
        }

        private void BooksTree_MouseMove(object sender, MouseEventArgs e)
        {
                StoreExpandedStateBooks();
                StoreExpandedStateUsers();
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                TreeView treeView = (TreeView)sender;
                TreeViewItem treeViewItem = FindTreeViewItem(e.OriginalSource as DependencyObject);

                try
                {
                    if (treeViewItem != null)
                    {
                        Book selectedBook = (Book)treeViewItem.Header;

                        if (selectedBook.Dostupnost)
                        {
                            DataObject dragData = new DataObject(typeof(Book), selectedBook);
                            DragDrop.DoDragDrop(treeViewItem, dragData, DragDropEffects.Move);
                        }
                    }
                }
                catch (Exception) { }
            }
        }

        private void UsersTree_Drop(object sender, DragEventArgs e)
        {
            TreeView treeView = (TreeView)sender;
            TreeViewItem targetUserNode = FindTreeViewItem(e.OriginalSource as DependencyObject);
            
            if (targetUserNode != null)
            {
                try
                {
                    User targetKorisnik = (User)targetUserNode.Header;
                    Book draggedBook = (Book)e.Data.GetData(typeof(Book));
                    BookUser bu = new BookUser(targetKorisnik.Id,draggedBook.Naslov,draggedBook.Autor);
                    bu.Append();
                

                targetKorisnik.IznajmljeneKnjige.Add(draggedBook);
                

                BooksTree.Items.Refresh();
                UsersTree.Items.Refresh();

                draggedBook.Dostupnost = false;
                draggedBook.NumRented = draggedBook.NumRented + 1;
                targetKorisnik.NumRented = targetKorisnik.NumRented + 1;
               
                }
                catch (Exception) { }
               
                k.Export();
                bookView.b.Export();


                RestoreExpandedStateBooks();
                RestoreExpandedStateUsers();

                
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

        private void StoreExpandedStateBooks()
        {
            expandedBooksState.Clear();
            StoreExpandedStateFunc(BooksTree, expandedBooksState);
        }

        private void StoreExpandedStateUsers()
        {
            expandedUsersState.Clear();
            StoreExpandedStateFunc(UsersTree, expandedUsersState);
        }

        private void StoreExpandedStateFunc(ItemsControl parentItem, Dictionary<object, bool> expandedState)
        {
            foreach (var item in parentItem.Items)
            {
                TreeViewItem treeViewItem = parentItem.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (treeViewItem != null)
                {
                    expandedState[item] = treeViewItem.IsExpanded;
                   
                }
            }


        }

        private void RestoreExpandedStateBooks()
        {
            RestoreExpandedStateFunc(BooksTree, expandedBooksState);
        }

        private void RestoreExpandedStateUsers()
        {
            RestoreExpandedStateFunc(UsersTree, expandedUsersState);
        }

        private void RestoreExpandedStateFunc(ItemsControl parentItem, Dictionary<object, bool> expandedState)
        {
            foreach (var item in parentItem.Items)
            {
                TreeViewItem treeViewItem = parentItem.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (treeViewItem != null)
                {
                    if (expandedState.TryGetValue(item, out bool isExpanded) && isExpanded)
                    {
                        treeViewItem.IsExpanded = true;
                    }
                    
                }
            }
        }

        private void RemoveUserFromDataBase(string lineToRemove)
        {
            string filePath = "../../Data/DataBase.txt";
            string tempFilePath = "../../Data/TempDataBase.txt";        
            
            using (StreamWriter writer = new StreamWriter(tempFilePath))
            {
                
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    string[] linePart;
                    while ((line = reader.ReadLine()) != null)
                    {
                        linePart = line.Split('|');
                        if (linePart[0] != lineToRemove)
                        {                           
                            writer.WriteLine(line);
                        }
                    }
                }
            }
            File.Delete(filePath);
            File.Move(tempFilePath, filePath);
        }

    }
}
