﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sebasol12Pieds
{
    [Serializable]
    public class HeatingSystem: IHeatingSystem, ISerializable
    {
        public HeatingSystem()
        {
            SolarPanel = new WaterHeater(null,null, 12.0/60.0, "Panneaux solaires");
            WaterStove = new WaterHeater(null, null, 12.0/60.0, "Poêle");
            Accumulator = new Accumulator(null, null, null);
            GazBoiler = new WaterHeater(null, null, 20.0 / 60.0, "Chaudière");
            Home = new Home(null, null);

            _menu = new Menu("******************* Sebasol 12 Pieds *******************");
            InitilizeMenu();
        }

        public WaterHeater SolarPanel { get; set; }
        public WaterHeater WaterStove { get; set; }
        public WaterHeater GazBoiler { get; set; }
        public Accumulator Accumulator { get; set; }
        [XmlIgnore]
        public Home Home { get; set; }

        [XmlIgnore]
        public string Infos
        {
            get
            {
                return ToString();
            }
        }

        public IAcumulator IAccumulator => Accumulator;

        public IWaterHeater ISolarPanel => SolarPanel;

        public IWaterHeater IWaterStove => WaterStove;

        public IWaterHeater IGazBoiler => GazBoiler;

        public IHome IHome => Home;

        public override string ToString()
        {
            return "Panneaux solaire : \n" + SolarPanel.ToString() + "\n\n" +
                   "Poele : \n" + WaterStove.ToString() + "\n\n" +
                   "Chaudière : \n" + GazBoiler.ToString() + "\n\n" +
                   "Accumulateur : \n" + Accumulator.ToString() + "\n\n" + 
                   "Maison : \n" + Home.ToString();
        }

        public void StartMenu()
        {
            _menu.Display();
        }

        private void InitilizeMenu()
        {
            _menu.AddOption(new Option("Visualiser l'état de l'instalation.", () =>
            {
                Console.Clear();
                Console.WriteLine(ToString());
                Console.WriteLine();
                Console.WriteLine("Pressez <enter> pour continuer ...");
                Console.ReadLine();
            }));

            _menu.AddOption(new Option("Modifier les paramètres des panneaux solaires.", () =>
            {
                SolarPanel.StartMenu();
            }));

            _menu.AddOption(new Option("Modifier les paramètres du poêle hydraulique.", () =>
            {
                WaterStove.StartMenu();
            }));

            _menu.AddOption(new Option("Modifier les paramètres de la chaudière.", () =>
            {
                GazBoiler.StartMenu();
            }));

            _menu.AddOption(new Option("Modifier les paramètres de l'accumulateur.", () =>
            {
                Accumulator.StartMenu();
            }));

            _menu.AddOption(new Option("Sauver les paramètres", () =>
            {
                SaveParameters();
                _menu.Display();
            }));

            _menu.AddOption(new Option("Modifier les paramètres de la maison.", () =>
            {
                Home.StartMenu();
            }));

            _menu.AddOption(new Option("Quitter.", () =>
            {
                _menu.Stop = true;
            }));
        }

        private void SaveParameters()
        {
            FileSerializer.Serialize("/home/pi/Programs/Config.dat", this);
        }

        protected HeatingSystem(SerializationInfo info, StreamingContext ctxt)
        {
            SolarPanel = (WaterHeater)info.GetValue("SolarPanel", typeof(WaterHeater));
            WaterStove = (WaterHeater)info.GetValue("WaterStove", typeof(WaterHeater));
            GazBoiler = (WaterHeater)info.GetValue("GazBoiler", typeof(WaterHeater));
            Accumulator = (Accumulator)info.GetValue("Accumulator", typeof(Accumulator));
            Home = (Home)info.GetValue("Home", typeof(Home));
            _menu = new Menu("******************* Sebasol 12 Pieds *******************");
            InitilizeMenu();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SolarPanel", SolarPanel, typeof(WaterHeater));
            info.AddValue("WaterStove", WaterStove, typeof(WaterHeater));
            info.AddValue("GazBoiler", GazBoiler, typeof(WaterHeater));
            info.AddValue("Accumulator", Accumulator, typeof(Accumulator));
            info.AddValue("Home", Home, typeof(Home));
        }

        private Menu _menu;
    }
}
