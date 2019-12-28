using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebasol12Pieds
{
   public class HeatingSystem
   {
        public HeatingSystem(List<Ds18b20> ds18B20s)
        {
            PanneauxSolaires = new SolarPanel(null,null, 12.0/60.0, ds18B20s);
            PoeleHydraulique = new WaterStove(null, null, 12.0/60.0, ds18B20s);
            Accumulateur = new Accumulator(null, null, null, ds18B20s);
            Chaudiere = new GazBoiler(null, null, 20.0 / 60.0, ds18B20s);
            Maison = new Home(null, ds18B20s);
            _ds18B20s = ds18B20s;

            _menu = new Menu("******************* Sebasol 12 Pieds *******************");
            InitilizeMenu();
        }

        public SolarPanel PanneauxSolaires { get; set; }
        public WaterStove PoeleHydraulique { get; set; }
        public GazBoiler Chaudiere { get; set; }
        public Accumulator Accumulateur { get; set; }
        public Home Maison { get; set; }

        public string Infos
        {
            get
            {
                return ToString();
            }
        }

        public override string ToString()
        {
            return "Panneaux solaire : \n" + PanneauxSolaires.ToString() + "\n\n" +
                   "Poele : \n" + PoeleHydraulique.ToString() + "\n\n" +
                   "Chaudière : \n" + Chaudiere.ToString() + "\n\n" +
                   "Accumulateur : \n" + Accumulateur.ToString() + "\n\n" + 
                   "Maison : \n" + Maison.ToString();
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
                PanneauxSolaires.StartMenu();
            }));

            _menu.AddOption(new Option("Modifier les paramètres du poêle hydraulique.", () =>
            {
                PoeleHydraulique.StartMenu();
            }));

            _menu.AddOption(new Option("Modifier les paramètres de la chaudière.", () =>
            {
                Chaudiere.StartMenu();
            }));

            _menu.AddOption(new Option("Modifier les paramètres de l'accumulateur.", () =>
            {
                Accumulateur.StartMenu();
            }));

            _menu.AddOption(new Option("Modifier les paramètres de la maison.", () =>
            {
                Maison.StartMenu();
            }));

            _menu.AddOption(new Option("Quitter.", () =>
            {
                _menu.Stop = true;
            }));
        }

        private Menu _menu;

        private List<Ds18b20> _ds18B20s;
    }
}
