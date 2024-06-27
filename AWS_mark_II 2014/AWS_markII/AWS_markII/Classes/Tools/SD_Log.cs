using System.IO;
using System;
using Microsoft.SPOT;
using System.Text;
using System.Threading;

namespace Tools
{
    class SD_Log : SD_card
    {
        private static string _nameFile;
        private static string _Message;

        public SD_Log()
            :base()
        { }
        public string Namefile
        {
            get { return _nameFile; }
            set { _nameFile = value; }
        }

        public string NoDest
        {
            get 
            {
                if (File.Exists(@"SD\" + "Settings" + Path.DirectorySeparatorChar + "NumCellDefault.txt"))
                {
                    return ReadString("Settings" + Path.DirectorySeparatorChar + "NumCellDefault.txt");
                }
                else { return "085218877358"; }
            }
        }

        private string NameCreateFile
        {
            get { return @"SD\" + _mypath + Path.DirectorySeparatorChar + _nameFile + ".csv"; }
        }

        public string ContentFile
        {
            get { return _Message; }
            set { _Message = value; }
        }

        private Boolean New()
        {
            try
            {

                FileStream filestream = new FileStream(NameCreateFile, FileMode.Create, FileAccess.Write, FileShare.None);
                if (_Message.Length > 0)
                {
                    StreamWriter streamWriter = new StreamWriter(filestream);
                    streamWriter.WriteLine(_Message);
                    Debug.Print("write a Line:" + _Message);
                    streamWriter.Close();
                }
                filestream.Close();
                Debug.Print(NameCreateFile + ": create!");
                return true;

            }

            catch (Exception ex)
            {
                Debug.Print(NameCreateFile + ": error!" + ex.Message);
                return false;
            }
        }

        private Boolean AppendFile()
        {
            try
            {

                FileStream filestream = new FileStream(NameCreateFile, FileMode.Append, FileAccess.Write, FileShare.None);

                if (_Message.Length > 0)
                {
                    StreamWriter streamWriter = new StreamWriter(filestream);
                    streamWriter.WriteLine(_Message);
                    Debug.Print("append a Line:" + _Message);
                    streamWriter.Close();
                }
                filestream.Close();
                Debug.Print(NameCreateFile + ": Write!");
                return true;

            }
            catch (Exception ex)
            {
                Debug.Print(NameCreateFile + ": error!" + ex.Message);
                return false;
            }
        }

        private Boolean FileExist()
        {
            try
            {
                if (File.Exists(NameCreateFile))
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
                if (!File.Exists(@"\SD\Report\" + file +".txt"))
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
                    streamWriter.WriteLine(TimeStamp+ " : "+ Message);
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
                Debug.Print("Writing data: " + NameCreateFile + " ...Error!!" + ex.Message);
                Report = ex.Message;
                return false;
            }
        }

        public static string Replace(string Content, string FromReplace, string Subtitution)
        {
            string NewString = "";

            if ((FromReplace.Length == 1) && (Subtitution.Length > 1))
            {
                string[] tmpContent = Content.Split(FromReplace.ToCharArray());
                for (int i = 0; i <= tmpContent.Length - 1; i++)
                {
                    NewString += tmpContent[i] + FromReplace;
                }
                return NewString;
            }
            else
            {
                StringBuilder tmpContent = new StringBuilder(Content);
                return tmpContent.Replace(FromReplace, Subtitution).ToString();
            }
        }
        public static string ReadString(string NameFile)
        {
            string FileContent = "";
            try
            {
                // READ FILE

                FileStream filestream = new FileStream(@"\SD\" + NameFile, FileMode.Open);
                StreamReader reader = new StreamReader(filestream);
                FileContent = reader.ReadToEnd();
                reader.Close();
                StringBuilder RemoveReturn = new StringBuilder(FileContent);
                FileContent = RemoveReturn.Replace("\r\n", "").ToString();
                return FileContent;
            }
            catch (Exception ex)
            {

                Thread.Sleep(5000);
                Debug.Print(ex.Message);
                return "";
            }
        }
        public static string strMID(string StringA, int Initial, int EndString)
        {
            string NewString = "";
            try
            {
                char[] chr = StringA.ToCharArray();
                for (int t = Initial; t <= EndString - 1; t++)
                {
                    NewString += chr[t];
                }
                return NewString;
            }
            catch
            {
                return "";
            }
        }
    }
    
}
