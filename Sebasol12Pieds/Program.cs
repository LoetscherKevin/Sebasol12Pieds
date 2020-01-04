using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            HeatingSystem heatingSystem;
            if (File.Exists("/home/pi/Programs/Config.dat"))
            {
                Console.WriteLine("Deserialization HeatingSystem with Congig.dat file.");
                heatingSystem = FileSerializer.Deserialize<HeatingSystem>("/home/pi/Programs/Config.dat");
            }
            else
            {
                Console.WriteLine("New HeatingSystem.");
                heatingSystem = new HeatingSystem();
            }

            _heatingSystem = heatingSystem;

            // Configure Timer
            _timer = new Timer();
            _timer.Elapsed += timer_Tick;
            _timer.Interval = MilliSecondsLeftTilTheMinute();
            _timer.Enabled = true;

            heatingSystem.StartMenu();
        }

        private static IHeatingSystem _heatingSystem;

        public static void timer_Tick(Object source, System.Timers.ElapsedEventArgs e)
        {
            CallBack();
            _timer.Interval = MilliSecondsLeftTilTheMinute();
        }

        private static void CallBack()
        {
            Console.Write("StartTime : " + DateTime.Now.ToString("HH:mm:ss.fff"));
            SaveMeasurement();
            Console.WriteLine("\tEndTime : " + DateTime.Now.ToString("HH:mm:ss.fff"));
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

            // Dropbox token MesuresSebasol12Pieds         : mwjty1tHx4AAAAAAAAAADQSNFW_lRmxl5YHKo286OHkUgvPw9hqalSfVSvQiMYlZ
            // Dropbox token kevin.loetscher1337@gmail.com : YVB8BFgSwpgAAAAAAAAdV7JrQdRZ7l0KxdfTgpZw85JvfDx-uchXAut--eYPikk0
            using (var dbx = new DropboxClient("mwjty1tHx4AAAAAAAAAADQSNFW_lRmxl5YHKo286OHkUgvPw9hqalSfVSvQiMYlZ"))
            {
                string fileName = "Mesures_" + measurement.DateTime.Month.ToString() + "_" + measurement.DateTime.Year.ToString() + ".csv";
                string content;
                if (FileExist(dbx, "", fileName))
                {
                    content = Download(dbx, "", fileName);
                    content += "\n";
                    content += measurement.ToString();
                    Upload(dbx, "", fileName, content);
                }
                else
                {
                    content = "Date - heure, Température accumulateur haut, Température accumulateur centre, Température accumulateur bas,  Température entrée panneaux solaires, Température sortie panneaux solaires, débit panneaux solaires, Température entrée poêle, Température sortie poêle, débit poêle, Température entrée chaudière, Température sortie chaudière, débit chaudière, Température intérieur maison, Température extérieur maison";
                    content += "\n";
                    content += measurement.ToString();
                    Upload(dbx, "", fileName, content);
                }
            }
        }

        static bool FileExist(DropboxClient dbx, string folder, string file)
        {
            try
            {
                Download(dbx, folder, file);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
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
