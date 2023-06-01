using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.Class
{
    class BookUser : ObservableObject
    {
        private string bookNaslov;
        private string bookAutor;
        private string userId;

        public BookUser(string userId, string bookNaslov, string bookAutor)
        {
            this.bookNaslov = bookNaslov;
            this.bookAutor = bookAutor;
            this.userId = userId;
        }

        public string BookNaslov
        {
            get { return bookNaslov; }
            set
            {
                if (bookNaslov != value)
                {
                    bookNaslov = value;
                    OnPropertyChanged("BookNaslov");
                }
            }
        }
        public string UserId
        {
            get { return userId; }
            set
            {
                if (userId != value)
                {
                    userId = value;
                    OnPropertyChanged("UserId");
                }
            }
        }
        public string BookAutor
        {
            get { return bookAutor; }
            set
            {
                if (bookAutor != value)
                {
                    bookAutor = value;
                    OnPropertyChanged("BookAutor");
                }
            }
        }
        
        
        public override string ToString()
        {
            string str = UserId +"|"+ BookNaslov +"|"+BookAutor;
            return str;
        }



        public void Append()
        {
            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter("../../Data/DataBase.txt", true);
                sw.WriteLine(ToString());
            }
            catch (Exception)
            {
                
            }
            finally
            {
                sw?.Close();
            }
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
                       
                    if(Id == userId)
                    {
                        knjige.Add(bookNaslov + " " + bookAutor);
                    }        
                }
                string mostCommon = "";
                mostCommon = knjige.GroupBy(item => item).OrderByDescending(group => group.Count()).Select(group => group.Key).FirstOrDefault();
                return mostCommon;
            }
            catch (Exception)
            {  }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    
                }
            }
            return "";
        }



    }

}
