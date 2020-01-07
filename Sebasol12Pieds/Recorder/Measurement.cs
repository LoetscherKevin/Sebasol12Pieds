using System;
using System.Collections.Generic;
using System.Globalization;
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
        public double HomeOutsideTemperature { get; set; }
        public override string ToString()
        {
            string delimiter = ";";
            string measurement = "";
            measurement += DateTime.ToString("dd.MM.yyyy") + delimiter;
            measurement += DateTime.ToString("HH:mm:ss") + delimiter;

            measurement += DateTime.Year.ToString() + delimiter;
            measurement += DateTime.Month.ToString() + delimiter;
            measurement += DateTime.Day.ToString() + delimiter;

            measurement += DateTime.Hour.ToString() + delimiter;
            measurement += DateTime.Minute.ToString() + delimiter;
            measurement += DateTime.Second.ToString() + delimiter;
            measurement += DateTime.Millisecond.ToString().ToString() + delimiter;

            measurement += AccumulatorTopTemperature.ToString() + delimiter;
            measurement += AccumulatorCenterTemperature.ToString() + delimiter;
            measurement += AccumulatorBottomTemperature.ToString() + delimiter;

            measurement += SolarPanelInputTemperature.ToString() + delimiter;
            measurement += SolarPanelOutputTemperature.ToString() + delimiter;
            measurement += SolarPanelFlow.ToString() + delimiter;

            measurement += WaterStoveInputTemperature.ToString() + delimiter;
            measurement += WaterStoveOutputTemperature.ToString() + delimiter;
            measurement += WaterStoveFlow.ToString() + delimiter;

            measurement += GazBoilerInputTemperature.ToString() + delimiter;
            measurement += GazBoilerOutputTemperature.ToString() + delimiter;
            measurement += GazBoilerFlow.ToString() + delimiter;

            measurement += HomeInsideTemperature.ToString() + delimiter;
            measurement += HomeOutsideTemperature.ToString();
            return measurement;
        }
    }
}
