using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Biblioteka
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static int idKorisnikaCnt = 1;
        public static int IdKorisnikaCnt { get => idKorisnikaCnt; set => idKorisnikaCnt = value; }
    }
}
