using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Biblioteka.Class
{
     public class Book : ObservableObject
    {
        private string sifra;
        private string naslov;
        private string autor;
        private string zanr;
        private bool dostupnost;
        private string image;
        private int numRented;
        
        

        public Book(string sifra = "0", string naslov="-Empty-", string autor="-Empty-", string zanr="-Empty-", bool dostupnost = true, string putanjaIkonica= "/Images/defultBook.png",int numRented = 0)
        {
            this.sifra = sifra;
            this.naslov = naslov;
            this.autor = autor;
            this.zanr = zanr;
            this.dostupnost = dostupnost;
            this.image = putanjaIkonica;
            this.numRented = numRented;
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

        public BitmapImage TrueFalse()
        {

            string projectDirectory = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName).FullName;
            string imagesDirectory = Path.Combine(projectDirectory, "Icons");
            string imagePath = Dostupnost ? Path.Combine(imagesDirectory, "True.png") : Path.Combine(imagesDirectory, "False.png");

            BitmapImage bmi = new BitmapImage(new Uri(imagePath));
            return bmi;
        }
        public BitmapImage TrueFalseIcon
        {
            get { return TrueFalse(); }
        }
        public string Sifra
        {
            get { return sifra; }
            set
            {
                if (sifra != value)
                {
                    sifra = value;
                    OnPropertyChanged("Sifra");
                }
            }
        }

        public string Naslov
        {
            get { return naslov; }
            set
            {
                if (naslov != value)
                {
                    naslov = value;
                    OnPropertyChanged("Naslov");
                }
            }
        }
    
        public string Autor
        {
            get { return autor; }
            set
                {
                if (autor != value)
                {
                    autor = value;
                    OnPropertyChanged("Autor");
                }
            }
        }

        public string Zanr
        {
            get { return zanr; }
            set
            {
                if (zanr != value)
                {
                    zanr = value;
                    OnPropertyChanged("Zanr");
                }
            }
        }
        public bool Dostupnost
        {
            get { return dostupnost; }
            set
            {
                if (dostupnost != value)
                {
                    dostupnost = value;
                    OnPropertyChanged("Dostupnost");
                }
            }
        }
        public Visibility InversDostupnost
        {
            get { return dostupnost ? Visibility.Collapsed : Visibility.Visible; }
        }
        public string ImagePath
        {
            get { return image; }
            set
            {
                if (image != value)
                {
                    image = value;
                    OnPropertyChanged("Image");
                }
            }
        }

        public override string ToString()
        {
            string path = @"\Images\";
            string[] ImagePathSplit = image.Split('\\');
            path += ImagePathSplit[ImagePathSplit.Length - 1];
            
            string str = "";
            string dost = "";

            if (dostupnost)
                dost += "DA";
            else
                dost += "NE";          

            str += sifra + "|" + naslov + "|" + autor + "|" + zanr + "|" + dost + "|" + path+"|"+NumRented.ToString();

            return str;
        }


    }
}
