using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Audalia.DataHUBCommon
{
    public class DataHubConfiguration
    {
        static IniFile configFile = null;


        public static string PathToCodeBase
        {
            get
            {
                var codeBaseUrl = Assembly.GetExecutingAssembly().CodeBase;
                var filePathToCodeBase = new Uri(codeBaseUrl).LocalPath;
                return Path.GetDirectoryName(filePathToCodeBase);
            }
        }

        static IniFile ConfigFile
        {
            get
            {
                if (configFile == null)
                {
                    var iniFileName = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\DataHub.ini";
                    configFile = new IniFile(iniFileName);
                }

                return configFile;
            }
        }

        public static string BinPath { get { return ConfigFile.Read("BinPath", "Service"); } }

        public static string BaseAddress { get { return ConfigFile.Read("BaseAddress", "Service"); } }
        
        /*
        public static string Read(string Key, string Section = null)
        {
            return ConfigFile.Read(Key, Section);
        }
        */
    }

    public class IniFile
    {
        string Path;
        string EXE = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public IniFile(string IniPath = null)
        {
            Path = new FileInfo(IniPath ?? EXE + ".ini").FullName;
        }

        public string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }

        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? EXE);
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? EXE);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
    }

}
