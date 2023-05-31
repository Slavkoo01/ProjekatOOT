using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Biblioteka.Class
{
    public class User : INotifyPropertyChanged
    {
        private int idKorisnika;
        private string ime;
        private string prezime;
        public ObservableCollection<Book> knjigeIznajmljene;

        public User(string ime, string prezime) 
        {
            idKorisnika = App.IdKorisnikaCnt;
            App.IdKorisnikaCnt++;

            Ime = ime;
            Prezime = prezime;

            knjigeIznajmljene = new ObservableCollection<Book>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public string Ime
        {
            get { return ime; }

            set {
                if (value != ime)
                {
                    ime = value;
                    OnPropertyChanged("Ime");
                }
            }
        }

        public string Prezime
        {
            get { return prezime; }

            set{
                if (value != prezime)
                {
                    prezime = value;
                    OnPropertyChanged("Prezime");
                }
            }
        }
        public override string ToString()
        {
            return $"{Ime} {Prezime} (ID: {IdKorisnika})";
        }
        public int IdKorisnika { get => idKorisnika; set => idKorisnika = value; }
    }
}
