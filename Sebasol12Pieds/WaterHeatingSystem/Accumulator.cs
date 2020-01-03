using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebasol12Pieds
{
    public class Accumulator : IAcumulator
    {
        public Accumulator(Ds18b20 topTemperatureSensor, Ds18b20 centerTemperatureSensor, Ds18b20 bottomTemperatureSensor)
        {
            TopTemperatureSensor = topTemperatureSensor;
            CenterTemperatureSensor = centerTemperatureSensor;
            BottomTemperatureSensor = bottomTemperatureSensor;

            _menu = new Menu("Accumulateur : ");
            InitilizeMenu();
        }

        public Ds18b20 TopTemperatureSensor { get; set; }
        public Ds18b20 CenterTemperatureSensor { get; set; }
        public Ds18b20 BottomTemperatureSensor { get; set; }

        public double TopTemperature
        {
            get
            {
                return TopTemperatureSensor == null ? double.NaN : TopTemperatureSensor.Temperature;
            }
        }

        public double CenterTemperature
        {
            get
            {
                return CenterTemperatureSensor == null ? double.NaN : CenterTemperatureSensor.Temperature;
            }
        }

        public double BottomTemperature
        {
            get
            {
                return BottomTemperatureSensor == null ? double.NaN : BottomTemperatureSensor.Temperature;
            }
        }

        public override string ToString()
        {
            return "Sonde du haut : " + (TopTemperatureSensor==null? "null": TopTemperatureSensor.ToString()) + "\n" +
                   "Sonde du centre : " + (CenterTemperatureSensor == null ? "null" : CenterTemperatureSensor.ToString()) + "\n" +
                   "Sonde du bas : " + (BottomTemperatureSensor == null ? "null" : BottomTemperatureSensor.ToString());
        }

        public void StartMenu()
        {
            _menu.Display();
        }

        private Menu _menu;

        private void InitilizeMenu()
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
                TopTemperatureSensor = Ds18b20Finder.DisplayMenuSelectDs18B20();
            }));

            _menu.AddOption(new Option("Modifier la sonde du centre.", () =>
            {
                CenterTemperatureSensor = Ds18b20Finder.DisplayMenuSelectDs18B20();
            }));

            _menu.AddOption(new Option("Modifier la sonde du bas.", () =>
            {
                BottomTemperatureSensor = Ds18b20Finder.DisplayMenuSelectDs18B20();
            }));

            _menu.AddOption(new Option("Modifier les paramètres de la sonde du haut.", () =>
            {
                if (TopTemperatureSensor != null)
                    TopTemperatureSensor.StartMenu();
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
                if (CenterTemperatureSensor != null)
                    CenterTemperatureSensor.StartMenu();
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
                if (BottomTemperatureSensor != null)
                    BottomTemperatureSensor.StartMenu();
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
