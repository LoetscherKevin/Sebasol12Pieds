using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebasol12Pieds
{
    public class Measurement
    {
        public DateTime DateTime { get; set; }

        // Accumulator
        public double AccumulatorTopTemperature { get; set; }
        public double AccumulatorCenterTemperature { get; set; }
        public double AccumulatorBottomTemperature { get; set; }

        // SolarPanel
        public double SolarPanelInputTemperature { get; set; }
        public double SolarPanelOutputTemperature { get; set; }
        public double SolarPanelFlow { get; set; }

        // WaterStove
        public double WaterStoveInputTemperature { get; set; }
        public double WaterStoveOutputTemperature { get; set; }
        public double WaterStoveFlow { get; set; }

        // GazBoiler
        public double GazBoilerInputTemperature { get; set; }
        public double GazBoilerOutputTemperature { get; set; }
        public double GazBoilerFlow { get; set; }

        // Home
        public double HomeInsideTemperature { get; set; }

        public override string ToString()
        {
            string measurement = "";
            measurement += DateTime.ToString() + ",";

            measurement += AccumulatorTopTemperature.ToString() + ",";
            measurement += AccumulatorCenterTemperature.ToString() + ",";
            measurement += AccumulatorBottomTemperature.ToString() + ",";

            measurement += SolarPanelInputTemperature.ToString() + ",";
            measurement += SolarPanelOutputTemperature.ToString() + ",";
            measurement += SolarPanelFlow.ToString() + ",";

            measurement += WaterStoveInputTemperature.ToString() + ",";
            measurement += WaterStoveOutputTemperature.ToString() + ",";
            measurement += WaterStoveFlow.ToString() + ",";

            measurement += GazBoilerInputTemperature.ToString() + ",";
            measurement += GazBoilerOutputTemperature.ToString() + ",";
            measurement += GazBoilerFlow.ToString() + ",";

            measurement += HomeInsideTemperature.ToString();

            return measurement;
        }
    }
}
