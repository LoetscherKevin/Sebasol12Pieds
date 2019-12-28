using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebasol12Pieds
{
    public abstract class WaterHeater
    {
        public WaterHeater(Ds18b20 inputTemperatureSensor, Ds18b20 outputTemperatureSensor, double flow, List<Ds18b20> ds18B20s)
        {
            SondeEntree = inputTemperatureSensor;
            SondeSortie = outputTemperatureSensor;
            Debit = flow;
            Menu = new Menu(Nom + ": ");
            InitilizeMenu(ds18B20s);
        }

        public double Debit { get; set; } // dm^3/s
        public double TemperatureEntree
        {
            get
            {
                return SondeEntree == null ? double.NaN : SondeEntree.Temperature;
            }
        }

        public double TemperatureSortie
        {
            get
            {
                return SondeSortie == null ? double.NaN : SondeSortie.Temperature;
            }
        }

        public double DifferenceTemperature
        {
            get
            {
                return (SondeSortie == null? double.NaN: TemperatureSortie) - (SondeEntree == null ? double.NaN : TemperatureEntree);
            }
        }

        public double Puissance
        {
            get
            {
                return DifferenceTemperature * Debit * 4187; // 4187 = contant of water heating J/kg/K
            }
        }

        public Ds18b20 SondeEntree { get; set; }
        public Ds18b20 SondeSortie { get; set; }

        public override string ToString()
        {
            return "Sonde d'entrée : " + (SondeEntree == null? "null": SondeEntree.ToString()) + "\n" +
                   "Sonde de sortie : " + (SondeSortie == null ? "null" : SondeSortie.ToString()) + "\n" +
                   "Débit : " + (Debit * 60.0).ToString() + " (l/min)\n" + 
                   "Différence de température : " + DifferenceTemperature + "\n" +
                   "Puissance : " + Puissance;
        }

        public void StartMenu()
        {
            Menu.Display();
        }

        protected abstract string Nom { get; }

        protected Menu Menu { get; }

        private void InitilizeMenu(List<Ds18b20> ds18B20s)
        {
            Menu.AddOption(new Option("Visualiser l'état.", () =>
            {
                Console.Clear();
                Console.WriteLine(Nom + " : ");
                Console.WriteLine(ToString());
                Console.WriteLine();
                Console.WriteLine("Pressez <enter> pour continuer ...");
                Console.ReadLine();
            }));

            Menu.AddOption(new Option("Choisir la sonde d'entrée.", () =>
            {
                SondeEntree = Ds18b20Finder.SelectDs18B20(ds18B20s);
            }));

            Menu.AddOption(new Option("Choisir la sonde de sortie.", () =>
            {
                SondeSortie = Ds18b20Finder.SelectDs18B20(ds18B20s);
            }));

            Menu.AddOption(new Option("Modifier les paramètres de la sonde d'entrée.", () =>
            {
                if (SondeEntree != null)
                    SondeEntree.StartMenu();
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
                if (SondeSortie != null)
                    SondeSortie.StartMenu();
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
                Debit = ConsoleUtils.ReadPositiveReal("Choisir le débit (l/min) : ") / 60.0;
            }));

            Menu.AddOption(new Option("Retour", () =>
            {
                Menu.Stop = true;
            }));
        }
    }
}
