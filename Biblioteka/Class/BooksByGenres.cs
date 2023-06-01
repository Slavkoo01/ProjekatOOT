using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.Class
{
    public class BooksByGenres
    {
        public string Zanr { get; set; }
        public ObservableCollection<Book> Knjige { get; set; }

        public BooksByGenres(string zanr, List<Book> knjige)
        {
            Zanr = zanr;
            Knjige = new ObservableCollection<Book>(knjige);
        }
    }
}
