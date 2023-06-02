using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.Class
{
    public class GenreNode
    {        
            public string Genre { get; set; }
            public ObservableCollection<Book> Books { get; set; }

            public GenreNode(string genre)
            {
                Genre = genre;
                Books = new ObservableCollection<Book>();
            }
        
    }
}
