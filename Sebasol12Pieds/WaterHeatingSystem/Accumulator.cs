using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebasol12Pieds
{
    public class Accumulator
    {
        public Accumulator(Ds18b20 topTemperatureSensor, Ds18b20 centerTemperatureSensor, Ds18b20 bottomTemperatureSensor, List<Ds18b20> ds18B20s)
        {
            SondeHaut = topTemperatureSensor;
            SondeCentre = centerTemperatureSensor;
            SondeBas = bottomTemperatureSensor;

            _menu = new Menu("Accumulateur : ");
            InitilizeMenu(ds18B20s);
        }

        public Ds18b20 SondeHaut { get; set; }
        public Ds18b20 SondeCentre { get; set; }
        public Ds18b20 SondeBas { get; set; }

        public double TemperatureHaut
        {
            get
            {
                return SondeHaut == null ? double.NaN : SondeHaut.Temperature;
            }
        }

        public double TemperatureCentre
        {
            get
            {
                return SondeCentre == null ? double.NaN : SondeCentre.Temperature;
            }
        }

        public double TemperatureBas
        {
            get
            {
                return SondeBas == null ? double.NaN : SondeBas.Temperature;
            }
        }

        public override string ToString()
        {
            return "Sonde du haut : " + (SondeHaut==null? "null": SondeHaut.ToString()) + "\n" +
                   "Sonde du centre : " + (SondeCentre == null ? "null" : SondeCentre.ToString()) + "\n" +
                   "Sonde du bas : " + (SondeBas == null ? "null" : SondeBas.ToString());
        }

        public void StartMenu()
        {
            _menu.Display();
        }

        private Menu _menu;

        private void InitilizeMenu(List<Ds18b20> ds18B20s)
        {
            _menu.AddOption(new Option("Visualiser l'état.", () =>
            {
                Console.Clear();
                Console.WriteLine(ToString());
                Console.WriteLine();
                Console.WriteLine("Pressez <enter> pour continuer ...");
                Console.ReadLine();
            }));

            _menu.AddOption(new Option("Modifier la sonde du haut.", () =>
            {
                SondeHaut = Ds18b20Finder.SelectDs18B20(ds18B20s);
            }));

            _menu.AddOption(new Option("Modifier la sonde du centre.", () =>
            {
                SondeCentre = Ds18b20Finder.SelectDs18B20(ds18B20s);
            }));

            _menu.AddOption(new Option("Modifier la sonde du bas.", () =>
            {
                SondeBas = Ds18b20Finder.SelectDs18B20(ds18B20s);
            }));

            _menu.AddOption(new Option("Modifier les paramètres de la sonde du haut.", () =>
            {
                if (SondeHaut != null)
                    SondeHaut.StartMenu();
                else
                {
                    Console.Clear();
                    Console.WriteLine("Erreur : Il n'existe pas de sonde du haut!\n         Il faut choisir une sonde du haut -> 2.");
                    Console.WriteLine();
                    Console.WriteLine("Pressez <enter> pour continuer ...");
                    Console.ReadLine();
                }
            }));

            _menu.AddOption(new Option("Modifier les paramètres de la sonde du centre.", () =>
            {
                if (SondeCentre != null)
                    SondeCentre.StartMenu();
                else
                {
                    Console.Clear();
                    Console.WriteLine("Erreur : Il n'existe pas de sonde du centre!\n         Il faut choisir une sonde du centre -> 3.");
                    Console.WriteLine();
                    Console.WriteLine("Pressez <enter> pour continuer ...");
                    Console.ReadLine();
                }
            }));

            _menu.AddOption(new Option("Modifier les paramètres de la sonde du bas.", () =>
            {
                if (SondeBas != null)
                    SondeBas.StartMenu();
                else
                {
                    Console.Clear();
                    Console.WriteLine("Erreur : Il n'existe pas de sonde du bas!\n         Il faut choisir une sonde du bas -> 4.");
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
    }
}
