using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebasol12Pieds
{
    public class SolarPanel : WaterHeater
    {
        public SolarPanel(Ds18b20 inputTemperatureSensor, Ds18b20 outputTemperatureSensor, double flow, List<Ds18b20> ds18B20s) : base(inputTemperatureSensor, outputTemperatureSensor, flow, ds18B20s)
        {

        }

        protected override string Nom { get { return "Panneaux solaire"; } }
    }
}
