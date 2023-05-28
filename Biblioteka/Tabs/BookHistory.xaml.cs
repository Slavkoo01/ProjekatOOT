using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;

namespace Biblioteka.Tabs
{
    public partial class BookHistory : UserControl
    {
        private static BookView bookView;

        public BookHistory()
        {
            InitializeComponent();

            // Create a new BookView instance if it's the first instance
            bookView = new BookView();

            BookHistoryTabela.ItemsSource = bookView.b.biblioteka;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {

            bookView = new BookView();

            BookHistoryTabela.ItemsSource = bookView.b.biblioteka;

        }
    }
}