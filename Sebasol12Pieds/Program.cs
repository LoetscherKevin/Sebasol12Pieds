using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopsa;
using System.Timers;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Sebasol12Pieds
{
    class Program
    {
        private static Timer _timer;
        private static IMongoCollection<Measurement> _measurementsCollection;
        static void Main(string[] args)
        {
            // Configure MongoDb
            var client = new MongoClient("mongodb://192.168.1.22:27017");
            var database = client.GetDatabase("Sebasol12Pieds");
            _measurementsCollection = database.GetCollection<Measurement>("Measurements");

            // Configure Timer
            _timer = new Timer();
            _timer.Elapsed += timer_Tick;
            _timer.Interval = MilliSecondsLeftTilTheMinute();
            _timer.Enabled = true;

            HeatingSystem heatingSystem = new HeatingSystem();
            _heatingSystem = heatingSystem;
            WoopsaServer server = new WoopsaServer(heatingSystem,8080);
            heatingSystem.StartMenu();
            server.Dispose();
        }

        private static IHeatingSystem _heatingSystem;

        public static void timer_Tick(Object source, System.Timers.ElapsedEventArgs e)
        {
            CallBack();
            _timer.Interval = MilliSecondsLeftTilTheMinute();
        }

        private static void CallBack()
        {
            Console.Write("StartTime : " + DateTime.Now.ToString("hh:mm:ss.fff"));

            _measurementsCollection.InsertOne(new Measurement()
            {
                DateTime = DateTime.Now,
                // Accumulator
                AccumulatorTopTemperature = _heatingSystem.IAccumulator.TopTemperature,
                AccumulatorCenterTemperature = _heatingSystem.IAccumulator.CenterTemperature,
                AccumulatorBottomTemperature = _heatingSystem.IAccumulator.BottomTemperature,

                // SolarPanel
                SolarPanelInputTemperature = _heatingSystem.ISolarPanel.InputTemperature,
                SolarPanelOutputTemperature = _heatingSystem.ISolarPanel.OutputTemperature,
                SolarPanelFlow = _heatingSystem.ISolarPanel.Flow,

                // WaterStove
                WaterStoveInputTemperature = _heatingSystem.IWaterStove.InputTemperature,
                WaterStoveOutputTemperature = _heatingSystem.IWaterStove.OutputTemperature,
                WaterStoveFlow = _heatingSystem.IWaterStove.Flow,

                // GazBoiler
                GazBoilerInputTemperature = _heatingSystem.IGazBoiler.InputTemperature,
                GazBoilerOutputTemperature = _heatingSystem.IGazBoiler.OutputTemperature,
                GazBoilerFlow = _heatingSystem.IGazBoiler.Flow,

                // Home
                HomeInsideTemperature = _heatingSystem.IHome.InsideTemperature
            });

            Console.WriteLine("\tEndTime : " + DateTime.Now.ToString("hh:mm:ss.fff"));
        }

        private static int MilliSecondsLeftTilTheMinute()
        {
            int secondsRemaining = 59 - DateTime.Now.Second;
            int millisecond = 999 - DateTime.Now.Millisecond;

            int interval;
            interval = secondsRemaining * 1000 + millisecond;

            if (interval == 0)
            {
                interval = 60 * 1000;
            }
            return interval;
        }
    }

    public class Measurement
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
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
    }
}
