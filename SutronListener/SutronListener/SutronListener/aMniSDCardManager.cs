using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.IO;
using System.Threading;

namespace SutronListener
{
    public class aMniSDCardManager
    {
        static InterruptPort sdCardStatusPort = new InterruptPort((Cpu.Pin)57, false, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeBoth);
        protected static bool _ready = true;
        protected static string _report;
        protected static string _mypath;
        protected static string _nameFile;
        private static string _Message;

        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        public static string Report
        {
            get { return _report; }
            set { _report = value; }
        }
        public bool Ready
        {
            get { return _ready; }
            set { _ready = value; }
        }
        
        public string Namefile
        {
            get { return _nameFile; }
            set { _nameFile = value; }
        }
        public string FullPathOfLoggedFile
        {
            //"sta0171"+hari+bln+thn+jam+mnt+".txt"

            get { return @"SD\" + _mypath + Path.DirectorySeparatorChar + _nameFile + ".txt"; }
        }
        public aMniSDCardManager()
        {

            sdCardStatusPort.OnInterrupt += sdCardStatusPort_OnInterrupt;
        }

        static void sdCardStatusPort_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            if (!sdCardStatusPort.Read())
            {
                _ready = true;
            }
            else { _ready = false; }
        }


        public bool CheckPath(string rootDir, string YDir, string MDir, string DDir)
        {

            try
            {
            Again:
                if (!Directory.Exists(@"\SD\" + rootDir))
                {
                    Directory.CreateDirectory(@"\SD\" + rootDir);
                    Thread.Sleep(150);

                    goto Again;
                }

                else if (!Directory.Exists(@"\SD\" + rootDir + Path.DirectorySeparatorChar + YDir))
                {
                    Directory.CreateDirectory(@"\SD\" + rootDir + Path.DirectorySeparatorChar + YDir);
                    Thread.Sleep(150);
                    goto Again;
                }

                else if (!Directory.Exists(@"\SD\" + rootDir + Path.DirectorySeparatorChar + YDir + Path.DirectorySeparatorChar + MDir))
                {
                    Directory.CreateDirectory(@"\SD\" + rootDir + Path.DirectorySeparatorChar + YDir + Path.DirectorySeparatorChar + MDir);
                    Thread.Sleep(150);
                    goto Again;
                }
                else if (!Directory.Exists(@"\SD\" + rootDir + Path.DirectorySeparatorChar + YDir + Path.DirectorySeparatorChar + MDir + Path.DirectorySeparatorChar + DDir ))
                {
                    Directory.CreateDirectory(@"\SD\" + rootDir + Path.DirectorySeparatorChar + YDir + Path.DirectorySeparatorChar + MDir + Path.DirectorySeparatorChar + DDir);
                    Thread.Sleep(150);
                    goto Again;
                }
                else
                {
                    _mypath = rootDir + Path.DirectorySeparatorChar + YDir + Path.DirectorySeparatorChar + MDir + Path.DirectorySeparatorChar + DDir;
                    return true;
                }


            }
            catch (Exception ex)
            {
                _report = ex.Message;
                return (false);
            }
        }


        private Boolean New()
        {
            try
            {

                using (FileStream filestream = new FileStream(FullPathOfLoggedFile, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    if (_Message.Length > 0)
                    {
                        StreamWriter streamWriter = new StreamWriter(filestream);
                        streamWriter.WriteLine(_Message);
                        Debug.Print("write a Line:" + _Message);
                        streamWriter.Close();
                    }
                    filestream.Close();
                    Debug.Print(FullPathOfLoggedFile + ": create!");
                    return true;
                }
            }

            catch (Exception ex)
            {
                Debug.Print(FullPathOfLoggedFile + ": error!" + ex.Message);
                return false;
            }
        }

        private Boolean AppendFile()
        {
            try
            {

                FileStream filestream = new FileStream(FullPathOfLoggedFile, FileMode.Append, FileAccess.Write, FileShare.None);

                if (_Message.Length > 0)
                {
                    StreamWriter streamWriter = new StreamWriter(filestream);
                    streamWriter.WriteLine(_Message);
                    Debug.Print("append a Line:" + _Message);
                    streamWriter.Close();
                }
                filestream.Close();
                Debug.Print(FullPathOfLoggedFile + ": Write!");
                return true;

            }
            catch (Exception ex)
            {
                Debug.Print(FullPathOfLoggedFile + ": error!" + ex.Message);
                return false;
            }
        }

        public Boolean FileExist()
        {
            try
            {
                if (File.Exists(FullPathOfLoggedFile))
                {
                    return true;
                }
                else
                { return false; }
            }
            catch (Exception ex)
            {
                Debug.Print(Namefile + ":error!" + ex.Message);
                return false;
            }

        }

        public Boolean Reporting(string Message)
        {
            try
            {
                FileStream filestream;
                string file = DateTime.Now.Date.ToString("ddmmyy");
                string TimeStamp = DateTime.Now.ToString("HH:mm:ss");
                if (!Directory.Exists(@"\SD\Report\")) { Directory.CreateDirectory(@"\SD\Report\"); }
                if (!File.Exists(@"\SD\Report\" + file + ".txt"))
                {
                    filestream = new FileStream(@"\SD\Report\" + file + ".txt", FileMode.Create, FileAccess.Write, FileShare.None);
                }

                else
                {
                    filestream = new FileStream(@"\SD\Report\" + file + ".txt", FileMode.Append, FileAccess.Write, FileShare.None);
                }


                if (Message.Length > 0)
                {
                    StreamWriter streamWriter = new StreamWriter(filestream);
                    streamWriter.WriteLine(TimeStamp + " : " + Message);
                    Debug.Print("Report: " + Message);
                    streamWriter.Close();
                }
                filestream.Close();

                return true;
            }

            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                Debug.Print("Report File is missing?" + "   Reporting is Failed!");
                return false;
            }
        }

        public Boolean Write()
        {
            try
            {
                if (FileExist())
                {
                    return AppendFile();
                }
                else
                {
                    return New();
                }
            }

            catch (Exception ex)
            {
                Debug.Print("Writing data: " + FullPathOfLoggedFile + " ...Error!!" + ex.Message);
                Report = ex.Message;
                return false;
            }
        }

    }
}
