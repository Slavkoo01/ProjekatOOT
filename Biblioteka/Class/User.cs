using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.Class
{
     public class User : ObservableObject
    {
        private string id;
        private string ime; 
        private string prezime;
        List<Book> iznajmljeneKnjige;
        private int numRented;
        private string omiljenaKnjiga = "";
        public User(string id, string ime = "-Empty-", string prezime = "-Empty-",int numRented = 0)
        {
            this.id = id;
            this.ime = ime;
            this.prezime = prezime;
            iznajmljeneKnjige = new List<Book>();
            this.numRented = numRented;
        }
        public void Dodaj(Book book)
        {
            iznajmljeneKnjige.Add(book);
        }
        public string Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }
        public string Ime
        {
            get { return ime; }
            set
            {
                if (ime != value)
                {
                    ime = value;
                    OnPropertyChanged("Ime");
                }
            }
        }
        public string Prezime
        {
            get { return prezime; }
            set
            {
                if (prezime != value)
                {
                    prezime = value;
                    OnPropertyChanged("Prezime");
                }
            }
        }
        public int NumRented
        {
            get { return numRented; }
            set
            {
                if (numRented != value)
                {
                    numRented = value;
                    OnPropertyChanged("NumRented");
                }
            }
        }      
        public string OmiljenaKnjiga
        {
            get { return omiljenaKnjiga; }
            set
            {
                if (omiljenaKnjiga != value)
                {
                    omiljenaKnjiga = value;
                    OnPropertyChanged("OmiljenaKnjiga");
                }
            }
        }
        public List<Book> IznajmljeneKnjige
        {
            get { return iznajmljeneKnjige; }
            set
            {
                if (iznajmljeneKnjige != value)
                {
                    iznajmljeneKnjige = value;
                    OnPropertyChanged("IznajmljeneKnjige");
                }
            }
        }

        public override string ToString()
        {
            string knjige = "";
            
            foreach(Book book in iznajmljeneKnjige)
            {
                knjige += book.ToString() + "*";
            }
            

            string str = "";

            str += Id + "/" + Ime + "/" + Prezime + "/" + NumRented.ToString() + "/" + knjige;

            return str;
        }





    }
}
