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
        private static string _dropboxToken;
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

            _dropboxToken = File.ReadAllText("/home/pi/Programs/DropboxConfig.txt");

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
            };

            // mwjty1tHx4AAAAAAAAAAFBRY1URwO_26TNyRcKDjmWxuoqAasEvomt6RPiSQHBks
            using (var dbx = new DropboxClient(_dropboxToken))
            {
                string fileName = "Mesures_" +  measurement.DateTime.Year.ToString() + "_" + (measurement.DateTime.Month<10?"0":"").ToString() + measurement.DateTime.Month.ToString() + ".csv";
                string content = "";
                if (FileExist(dbx, "", fileName))
                {
                    content = Download(dbx, "", fileName);
                    content += "\n";
                    content += measurement.ToString();
                    Upload(dbx, "", fileName, content);
                }
                else
                {
                    List<string> columnName = new List<string>()
                    {
                        "Date", 
                        "Heure",
                        "Annee",
                        "Mois",
                        "Jour",
                        "Heure",
                        "Minute",
                        "Seconde",
                        "Milliseconde",
                        "Tmp accu haut", 
                        "Tmp accu centre", 
                        "Tmp accu bas",  
                        "Tmp sol in", 
                        "Tmp sol out", 
                        "Debit sol",
                        "Tmp poele in",
                        "Tmp poele out", 
                        "Debit poele", 
                        "Tmp gaz in", 
                        "Tmp gaz out", 
                        "Debit gaz", 
                        "Tmp maison int", 
                        "Tmp maison ext"
                    };

                    for (int i = 0; i < columnName.Count; i++)
                    {
                        if (i == columnName.Count - 1)
                            content += columnName[i];
                        else
                            content += columnName[i] + ";";
                    }
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
