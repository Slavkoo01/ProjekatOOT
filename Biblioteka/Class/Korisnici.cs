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
    public class Korisnici
    {
        public ObservableCollection<User> korisnici;
        public Korisnici()
        {
            korisnici = new ObservableCollection<User>();
        }

        public bool Dodaj(User b)
        {

           foreach (User b1 in korisnici)
            {
                if (b.Id.Equals(b1.Id))
                {
                    MessageBox.Show("Postoji knjiga sa ovom sifrom");
                    return false;
                }
            }
            korisnici.Add(b);
            return true;
        }
        public void Obrisi(int index)
        {
            korisnici.RemoveAt(index);
        }

        public override string ToString()
        {
            string str = "";
            foreach (User b in korisnici)
            {
                str += b + "\n";
            }
            return str;
        }

        public void Append(User user)
        {
            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter("../../Data/Korisnici.txt", true);
                sw.WriteLine(user.ToString());
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
                sw = new StreamWriter("../../Data/Korisnici.txt");
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

        public bool IdPostoji(string sifra)
        {
            foreach (User b in korisnici)
            {
                if (b.Id.Equals(sifra))
                
                    return true;
            }
            
            return false;
        }
        private List<Book> strToOj(string books)
        {
            List<Book> knjige = new List<Book>();
            string[] lineParts = books.Split('*');
            string sifra;
            string naslov;
            string autor;
            string zanr;
            string dost;
            string image;
            
            for (int i = 0; i < lineParts.Length-1; i++)
            {
                string[] bookParts = lineParts[i].Split('|');
                sifra = bookParts[0];
                naslov = bookParts[1];
                autor = bookParts[2];
                zanr = bookParts[3];
                dost = bookParts[4];
                image = bookParts[5];
                string projectDirectory = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName).FullName;
                projectDirectory += image;
                
                knjige.Add(new Book(sifra, naslov, autor, zanr, Dostupno(dost), projectDirectory));
                
            }
            return knjige; 
        }
        public void Import()
        {
           
            StreamReader sr = null;
            string id;
            string ime;
            string prezime;
            int numRented;
            string knjige;
            string linija;
            try
            {
                sr = new StreamReader("../../Data/Korisnici.txt");

                // petlja za kreiranje stavki (u fajlu je jedan red - jedna stavka)
                while ((linija = sr.ReadLine()) != null)
                {
                    //razdvajanje po delimiteru |
                    string[] lineParts = linija.Split('/');
                    id = lineParts[0];
                    ime = lineParts[1];
                    prezime = lineParts[2];
                    if (int.TryParse(lineParts[3], out int parsedNumRented))
                    {
                        numRented = parsedNumRented;
                    }
                    else
                    {
                        numRented = 0;
                    }
                    knjige = lineParts[4];
                    
                    List<Book> books = strToOj(knjige);
                    

                    if (!IdPostoji(id)) {
                        User k1 = new User(id, ime, prezime, numRented);
                        foreach(Book b in books)
                        {
                            k1.Dodaj(b);
                        }
                        
                        korisnici.Add(k1);
                    
                    
                    }
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
