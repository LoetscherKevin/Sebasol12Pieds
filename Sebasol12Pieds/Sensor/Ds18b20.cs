﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Sebasol12Pieds
{
    [Serializable]
    public class Ds18b20 : ISerializable
    {
        public Ds18b20(DirectoryInfo directoryInfo)
        {
            _directoryInfo = directoryInfo;
            
            _menu = new Menu("Sonde " + Name + " :");
            InitializeMenu();
        }

        public Ds18b20(string name)
        {
            DirectoryInfo devicesDir = new DirectoryInfo("/../sys/bus/w1/devices");
            foreach (var deviceDir in devicesDir.EnumerateDirectories("28*"))
            {
                if (deviceDir.Name == name)
                    _directoryInfo = deviceDir;
            }

            if (_directoryInfo == null)
                throw new Exception("Canot find temperature sensor ds18B20 id on /../sys/bus/w1/devices.");

            _menu = new Menu("Sonde " + Name + " :");
            InitializeMenu();
        }

        private DirectoryInfo _directoryInfo;
        public double Temperature
        {
            get
            {
                if (_lastDateTimeTemperatureReading == null || (DateTime.Now - _lastDateTimeTemperatureReading).TotalSeconds > 10)

                {
                    var w1slavetext = _directoryInfo.GetFiles("w1_slave").FirstOrDefault().OpenText().ReadToEnd();
                    string temperatureText = w1slavetext.Split(new string[] { "t=" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    _lastTemperature = double.Parse(temperatureText) / 1000 + Offset;
                    _lastDateTimeTemperatureReading = DateTime.Now;
                    return _lastTemperature;
                }
                else
                {
                    return _lastTemperature;
                }
            }
        }

        private double _lastTemperature;
        private DateTime _lastDateTimeTemperatureReading;

        public string Name
        {
            get
            {
                return _directoryInfo.Name;
            }
        }

        public double Offset { get; set; }

        public override string ToString()
        {
            return "ID : " + Name + "   Temperature : " + Temperature + "   Offset : " + Offset;
        }

        public void StartMenu()
        {
            _menu.Display();
        }

        private void InitializeMenu()
        {
            _menu.AddOption(new Option("Visualiser l'état.", () =>
            {
                Console.Clear();
                Console.WriteLine("Sonde " + Name + " : ");
                Console.WriteLine(ToString());
                Console.WriteLine();
                Console.WriteLine("Pressez <enter> pour continuer ...");
                Console.ReadLine();
            }));

            _menu.AddOption(new Option("Modifier l'offset.", () =>
            {
                Offset = ConsoleUtils.ReadReal("Choisir une valeur d'offet : ");
            }));

            _menu.AddOption(new Option("Retour.", () =>
            {
                _menu.Stop = true;
            }));
        }

        protected Ds18b20(SerializationInfo info, StreamingContext ctxt)
        {
            string name = info.GetString("Name");
            Offset = info.GetDouble("Offset");

            DirectoryInfo devicesDir = new DirectoryInfo("/../sys/bus/w1/devices");
            foreach (var deviceDir in devicesDir.EnumerateDirectories("28*"))
            {
                if (deviceDir.Name == name)
                    _directoryInfo = deviceDir;
            }

            if (_directoryInfo == null)
                throw new Exception("Canot find temperature sensor ds18B20 id on /../sys/bus/w1/devices.");

            _menu = new Menu("Sonde " + Name + " :");
            InitializeMenu();
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name, typeof(string));
            info.AddValue("Offset", Offset, typeof(double));
        }

        private Menu _menu;
    }
}
