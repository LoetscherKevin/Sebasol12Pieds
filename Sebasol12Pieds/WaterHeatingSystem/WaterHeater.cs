using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebasol12Pieds
{
    public class WaterHeater : IWaterHeater
    {
        public WaterHeater(Ds18b20 inputTemperatureSensor, Ds18b20 outputTemperatureSensor, double flow, string name)
        {
            InputTemperatureSensor = inputTemperatureSensor;
            OutputTemperatureSensor = outputTemperatureSensor;
            Flow = flow;
            Name = name;
            Menu = new Menu(Name + ": ");
            InitilizeMenu();
        }

        public double Flow { get; set; } // dm^3/s
        public double InputTemperature
        {
            get
            {
                return InputTemperatureSensor == null ? double.NaN : InputTemperatureSensor.Temperature;
            }
        }

        public double OutputTemperature
        {
            get
            {
                return OutputTemperatureSensor == null ? double.NaN : OutputTemperatureSensor.Temperature;
            }
        }

        public double TemperatureDelta
        {
            get
            {
                return (OutputTemperatureSensor == null? double.NaN: OutputTemperature) - (InputTemperatureSensor == null ? double.NaN : InputTemperature);
            }
        }

        public double Power
        {
            get
            {
                return TemperatureDelta * Flow * 4187; // 4187 = contant of water heating J/kg/K
            }
        }

        public Ds18b20 InputTemperatureSensor { get; set; }
        public Ds18b20 OutputTemperatureSensor { get; set; }

        public override string ToString()
        {
            return "Sonde d'entrée : " + (InputTemperatureSensor == null? "null": InputTemperatureSensor.ToString()) + "\n" +
                   "Sonde de sortie : " + (OutputTemperatureSensor == null ? "null" : OutputTemperatureSensor.ToString()) + "\n" +
                   "Débit : " + (Flow * 60.0).ToString() + " (l/min)\n" + 
                   "Différence de température : " + TemperatureDelta + "\n" +
                   "Puissance : " + Power;
        }

        public void StartMenu()
        {
            Menu.Display();
        }

        protected string Name { get; }

        protected Menu Menu { get; }

        private void InitilizeMenu()
        {
            Menu.AddOption(new Option("Visualiser l'état.", () =>
            {
                Console.Clear();
                Console.WriteLine(Name + " : ");
                Console.WriteLine(ToString());
                Console.WriteLine();
                Console.WriteLine("Pressez <enter> pour continuer ...");
                Console.ReadLine();
            }));

            Menu.AddOption(new Option("Choisir la sonde d'entrée.", () =>
            {
                InputTemperatureSensor = Ds18b20Finder.DisplayMenuSelectDs18B20();
            }));

            Menu.AddOption(new Option("Choisir la sonde de sortie.", () =>
            {
                OutputTemperatureSensor = Ds18b20Finder.DisplayMenuSelectDs18B20();
            }));

            Menu.AddOption(new Option("Modifier les paramètres de la sonde d'entrée.", () =>
            {
                if (InputTemperatureSensor != null)
                    InputTemperatureSensor.StartMenu();
                else
                {
                    Console.Clear();
                    Console.WriteLine("Erreur : Il n'existe pas de sonde d'entrée!\n         Il faut choisir une sonde d'entrée -> 2.");
                    Console.WriteLine();
                    Console.WriteLine("Pressez <enter> pour continuer ...");
                    Console.ReadLine();
                }
            }));

            Menu.AddOption(new Option("Modifier les paramètres de la sonde de sortie.", () =>
            {
                if (OutputTemperatureSensor != null)
                    OutputTemperatureSensor.StartMenu();
                else
                {
                    Console.Clear();
                    Console.WriteLine("Erreur : Il n'existe pas de sonde de sortie!\n         Il faut choisir une sonde de sortie -> 3.");
                    Console.WriteLine();
                    Console.WriteLine("Pressez <enter> pour continuer ...");
                    Console.ReadLine();
                }
            }));

            Menu.AddOption(new Option("Modifier le débit.", () =>
            {
                Flow = ConsoleUtils.ReadPositiveReal("Choisir le débit (l/min) : ") / 60.0;
            }));

            Menu.AddOption(new Option("Retour", () =>
            {
                Menu.Stop = true;
            }));
        }
    }
}
