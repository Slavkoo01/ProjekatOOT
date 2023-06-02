using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System.IO;
using Biblioteka.Class;


namespace Biblioteka.Tabs
{
    public partial class BookHistory : UserControl
    {
        private static BookView bookView;
        private static BookRent bookRent;

        public BookHistory()
        {
            InitializeComponent();
            
            bookView = new BookView();
            bookRent = new BookRent();

            UserHistoryTabela.ItemsSource = bookRent.k.korisnici;
            BookHistoryTabela.ItemsSource = bookView.b.biblioteka;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {

            bookView = new BookView();
            bookRent = new BookRent();

            foreach(User u in bookRent.k.korisnici)
            {
                u.OmiljenaKnjiga = FavouriteBook(u.Id);              
            }




            UserHistoryTabela.ItemsSource = bookRent.k.korisnici;
            BookHistoryTabela.ItemsSource = bookView.b.biblioteka;

        }
        public string FavouriteBook(string Id)
        {
            StreamReader sr = null;
            string userId;
            string bookNaslov;
            string bookAutor;
            string linija;
            List<string> knjige = new List<string>();
            try
            {
                sr = new StreamReader("../../Data/DataBase.txt");

                // petlja za kreiranje stavki (u fajlu je jedan red - jedna stavka)
                while ((linija = sr.ReadLine()) != null)
                {
                    //razdvajanje po delimiteru |
                    string[] lineParts = linija.Split('|');
                    userId = lineParts[0];
                    bookNaslov = lineParts[1];
                    bookAutor = lineParts[2];
                    
                    if (Id == userId)
                    {
                        knjige.Add(bookNaslov + " - " + bookAutor);
                    }
                }
                string mostCommon = "";
                mostCommon = knjige.GroupBy(item => item).OrderByDescending(group => group.Count()).Select(group => group.Key).FirstOrDefault();
                return mostCommon;
            }
            catch (Exception)
            { }
            finally
            {
                if (sr != null)
                {
                    sr.Close();

                }
            }
            return "";
        }

        private void ExportHistory_Click(object sender, RoutedEventArgs e)
        {
            bookView.b.Export();
            bookRent.k.Export();
        }
        
    }
}