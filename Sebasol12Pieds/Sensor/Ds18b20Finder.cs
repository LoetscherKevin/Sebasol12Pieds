using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebasol12Pieds
{
    public static class Ds18b20Finder
    { 
        public static List<Ds18b20> FindDs18b20()
        {
            List<Ds18b20> ds18B20Sensors = new List<Ds18b20>();
            DirectoryInfo devicesDir = new DirectoryInfo("/../sys/bus/w1/devices");
            foreach (var deviceDir in devicesDir.EnumerateDirectories("28*"))
            {
                ds18B20Sensors.Add(new Ds18b20(deviceDir));
            }

            return ds18B20Sensors;
        }

        public static Ds18b20 SelectDs18B20(List<Ds18b20> ds18B20s)
        {
            int sensorNumber = 1;
            int choice;
            Console.Clear();

            Console.WriteLine("Liste des sondes disponibles");
            foreach (var ds18B20 in ds18B20s)
            {
                Console.WriteLine(sensorNumber + ". Name : " + ds18B20.Name + "   Temperature : " + ds18B20.Temperature);
                sensorNumber++;
            }
            Console.WriteLine((ds18B20s.Count + 1).ToString() + ". Aucune");
            Console.WriteLine("Selectionner une sonde (" + 1 + "-" + (ds18B20s.Count + 1).ToString() + ")");

            choice = ConsoleUtils.ReadInt(1, ds18B20s.Count + 1);

            if (choice == (ds18B20s.Count + 1))
                return null;
            else
                return ds18B20s[choice - 1];
        }
    }
}
