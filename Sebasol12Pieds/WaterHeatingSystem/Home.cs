using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebasol12Pieds
{
    public class Home : IHome
    {
        public Home(Ds18b20 ds18B20)
        {
            InsideTemperatureSensor = ds18B20;

            _menu = new Menu("Maison : ");
            InitilizeMenu();
        }

        public Ds18b20 InsideTemperatureSensor { get; set; }

        public double InsideTemperature
        {
            get
            {
                return InsideTemperatureSensor == null ? double.NaN: InsideTemperatureSensor.Temperature;
            }
        }

        public override string ToString()
        {
            return "Sonde  : " + (InsideTemperatureSensor == null ? "null" : InsideTemperatureSensor.ToString());
        }

        public void StartMenu()
        {
            _menu.Display();
        }

        private void InitilizeMenu()
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
                InsideTemperatureSensor = Ds18b20Finder.DisplayMenuSelectDs18B20();
            }));

            _menu.AddOption(new Option("Modifier les paramètres de la sonde d'intérieur.", () =>
            {
                if (InsideTemperatureSensor != null)
                    InsideTemperatureSensor.StartMenu();
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
