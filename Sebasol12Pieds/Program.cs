using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopsa;

namespace Sebasol12Pieds
{
    class Program
    {
        static void Main(string[] args)
        {
            HeatingSystem heatingSystem = new HeatingSystem(Ds18b20Finder.FindDs18b20());
            WoopsaServer server = new WoopsaServer(heatingSystem,8080);
            heatingSystem.StartMenu();
            server.Dispose();
        }
    }
}
