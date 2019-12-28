using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebasol12Pieds
{
    public class Home
    {
        public Home(Ds18b20 ds18B20, List<Ds18b20> ds18B20s)
        {
            SondeInterieur = ds18B20;

            _menu = new Menu("Maison : ");
            InitilizeMenu(ds18B20s);
        }

        public Ds18b20 SondeInterieur { get; set; }

        public double Temperature
        {
            get
            {
                return SondeInterieur.Temperature;
            }
        }

        public override string ToString()
        {
            return "Sonde  : " + (SondeInterieur == null ? "null" : SondeInterieur.ToString());
        }

        public void StartMenu()
        {
            _menu.Display();
        }

        private void InitilizeMenu(List<Ds18b20> ds18B20s)
        {
            _menu.AddOption(new Option("Visualiser l'état.", () =>
            {
                Console.Clear();
                Console.WriteLine("Maison : ");
                Console.WriteLine(ToString());
                Console.WriteLine();
                Console.WriteLine("Pressez <enter> pour continuer ...");
                Console.ReadLine();
            }));

            _menu.AddOption(new Option("Choisir la sonde d'intérieur.", () =>
            {
                SondeInterieur = Ds18b20Finder.SelectDs18B20(ds18B20s);
            }));

            _menu.AddOption(new Option("Modifier les paramètres de la sonde d'intérieur.", () =>
            {
                if (SondeInterieur != null)
                    SondeInterieur.StartMenu();
                else
                {
                    Console.Clear();
                    Console.WriteLine("Erreur : Il n'existe pas de sonde d'intérieur!\n         Il faut choisir une sonde d'intérieur -> 2.");
                    Console.WriteLine();
                    Console.WriteLine("Pressez <enter> pour continuer ...");
                    Console.ReadLine();
                }
            }));

            _menu.AddOption(new Option("Retour.", () =>
            {
                _menu.Stop = true;
            }));
        }

        private Menu _menu;
    }
}
