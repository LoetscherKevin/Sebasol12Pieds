using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sebasol12Pieds
{
    [Serializable]
    public class Home : IHome, ISerializable
    {
        public Home(Ds18b20 insideTemperatureSensor, Ds18b20 outsideTemperatureSensor)
        {
            InsideTemperatureSensor = insideTemperatureSensor;
            OutsideTemperatureSensor = outsideTemperatureSensor;
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

        public Ds18b20 OutsideTemperatureSensor { get; set; }

        public double OutsideTemperature
        {
            get
            {
                return OutsideTemperatureSensor == null ? double.NaN : OutsideTemperatureSensor.Temperature;
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

            _menu.AddOption(new Option("Choisir la sonde d'extérieur.", () =>
            {
                OutsideTemperatureSensor = Ds18b20Finder.DisplayMenuSelectDs18B20();
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

            _menu.AddOption(new Option("Modifier les paramètres de la sonde extérieur.", () =>
            {
                if (OutsideTemperatureSensor != null)
                    OutsideTemperatureSensor.StartMenu();
                else
                {
                    Console.Clear();
                    Console.WriteLine("Erreur : Il n'existe pas de sonde d'extérieur!\n         Il faut choisir une sonde d'extérieur -> 2.");
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

        protected Home(SerializationInfo info, StreamingContext ctxt)
        {
            InsideTemperatureSensor = (Ds18b20)info.GetValue("InsideTemperatureSensor", typeof(Ds18b20));
            OutsideTemperatureSensor = (Ds18b20)info.GetValue("OutsideTemperatureSensor", typeof(Ds18b20));

            _menu = new Menu("Maison : ");
            InitilizeMenu();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("InsideTemperatureSensor", InsideTemperatureSensor, typeof(Ds18b20));
            info.AddValue("OutsideTemperatureSensor", OutsideTemperatureSensor, typeof(Ds18b20));
        }
    }
}
