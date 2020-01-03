using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopsa;
using System.Timers;
using Dropbox.Api;
using Dropbox.Api.Files;
using System.IO;

namespace Sebasol12Pieds
{
    class Program
    {
        private static Timer _timer;
        static void Main(string[] args)
        {
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
            SaveMeasurement();
            Console.WriteLine("\tEndTime : " + DateTime.Now.ToString("hh:mm:ss.fff"));
        }

        private static void SaveMeasurement()
        {
            Measurement measurement = new Measurement()
            {
                DateTime = DateTime.Now,
                // Accumulator
                AccumulatorTopTemperature = _heatingSystem.IAccumulator.TopTemperature,
                AccumulatorCenterTemperature = _heatingSystem.IAccumulator.CenterTemperature,
                AccumulatorBottomTemperature = _heatingSystem.IAccumulator.BottomTemperature,

                //// SolarPanel
                SolarPanelInputTemperature = _heatingSystem.ISolarPanel.InputTemperature,
                SolarPanelOutputTemperature = _heatingSystem.ISolarPanel.OutputTemperature,
                SolarPanelFlow = _heatingSystem.ISolarPanel.Flow,

                //// WaterStove
                WaterStoveInputTemperature = _heatingSystem.IWaterStove.InputTemperature,
                WaterStoveOutputTemperature = _heatingSystem.IWaterStove.OutputTemperature,
                WaterStoveFlow = _heatingSystem.IWaterStove.Flow,

                //// GazBoiler
                GazBoilerInputTemperature = _heatingSystem.IGazBoiler.InputTemperature,
                GazBoilerOutputTemperature = _heatingSystem.IGazBoiler.OutputTemperature,
                GazBoilerFlow = _heatingSystem.IGazBoiler.Flow,

                //// Home
                HomeInsideTemperature = _heatingSystem.IHome.InsideTemperature
            };

            using (var dbx = new DropboxClient("YVB8BFgSwpgAAAAAAAAdV7JrQdRZ7l0KxdfTgpZw85JvfDx-uchXAut--eYPikk0"))
            {
                string dropboxFile = Download(dbx, "", "Mesures.csv");
                dropboxFile += "\n";
                dropboxFile += measurement.ToString();
                Upload(dbx, "", "Mesures.csv", dropboxFile);
            }
        }

        static void Upload(DropboxClient dbx, string folder, string file, string content)
        {
            using (var mem = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                var task = dbx.Files.UploadAsync(
                    folder + "/" + file,
                    WriteMode.Overwrite.Instance,
                    body: mem);
                task.Wait();
            }
        }

        static string Download(DropboxClient dbx, string folder, string file)
        {
            string result = "";

            var taskFindFile = dbx.Files.DownloadAsync(folder + "/" + file);
            var response = taskFindFile.Result;
            taskFindFile.Wait();

            var taskGetContent = response.GetContentAsStringAsync();
            result = taskGetContent.Result;
            taskGetContent.Wait();

            response.Dispose();

            return result;
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
}
