using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Biblioteka.Class
{
    public class Biblioteka
    {
        public ObservableCollection<Book> biblioteka;
        public Biblioteka()
        {
            biblioteka = new ObservableCollection<Class.Book>();
        }

        public bool Dodaj(Book b)
        {
            
            foreach(Book b1 in biblioteka)
            {
                if (b.Sifra.Equals(b1.Sifra))
                {
                    MessageBox.Show("Postoji knjiga sa ovom sifrom");
                    return false;
                } 
            }
            biblioteka.Add(b);
            return true;
        }
        public void Obrisi(int index)
        {
            biblioteka.RemoveAt(index);
        }

        public override string ToString()
        {
            string str = "";
            foreach(Book b in biblioteka)
            {
                str += b + "\n";
            }
            return str;
        }





        public void Append(Book book)
        {
            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter("../../Data/Knjige.txt", true);              
                sw.WriteLine(book.ToString());
            }
            catch (Exception e)
            {
                var o = e;
            }
            finally
            {
                sw?.Close(); 
            }
        }
        public void Export()
        {
            StreamWriter sw = null;
            
            try
            {
                sw = new StreamWriter("../../Data/Knjige.txt");
                sw.Write(ToString());
                sw.Close();
            }
            catch (Exception e)
            {
                var o = e;
            }
        }
        private bool Dostupno(string str)
        {
            if (str.Equals("DA"))
                return true;
            else
                return false;
        }
        
        public bool SifraPostoji(string sifra)
        {
            foreach(Book b in biblioteka)
            {
                if (b.Sifra.Equals(sifra))
                    return true;
            }
            return false;
        }
        
        public void Import()
        {
            StreamReader sr = null;
            string sifra;
            string naslov;
            string autor;
            string zanr;
            string dost;
            string image;
            string linija;
            try
            {
                sr = new StreamReader("../../Data/Knjige.txt");

                // petlja za kreiranje stavki (u fajlu je jedan red - jedna stavka)
                while ((linija = sr.ReadLine()) != null)
                {
                    //razdvajanje po delimiteru |
                    string[] lineParts = linija.Split('|');
                    sifra = lineParts[0];
                    naslov = lineParts[1];
                    autor = lineParts[2];
                    zanr = lineParts[3];
                    dost = lineParts[4];
                    image = lineParts[5];
                    string projectDirectory = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName).FullName;
                    projectDirectory += image;
                    if (!SifraPostoji(sifra))
                        biblioteka.Add(new Book(sifra, naslov,autor,zanr,Dostupno(dost), projectDirectory));
                }
            }
            catch (Exception e)
            {
                var o = e;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
        }
        
    }
}
