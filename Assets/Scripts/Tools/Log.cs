using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Tools
{
    public class Log
    {
        public static void ERRO(string format)
        {
            Record(Level.ERRO, format);
        }

        public static void WARN(string format)
        {
            Record(Level.WARN, format);
        }

        public static void INFO(string format)
        {
            Record(Level.INFO, format);
        }

        public static void DEBU(string format)
        {
            Record(Level.DEBU, format);
        }

        internal enum Level
        {
            ERRO,
            WARN,
            INFO,
            DEBU,
        }

        internal static void Record(Level level, string v)
        {
            if(File.Exists(path) && new FileInfo(path).Length > 10*1024*1024)
            {
                var bakfile = path + ".bak";
                if(File.Exists(bakfile))
                {
                    File.Delete(bakfile);
                }

                File.Replace(path, bakfile, "");
            }

            if (level <= Level.WARN)
            {
                StackTrace st = new StackTrace(new StackFrame(true));

                var stackTrance = st.GetFrames().Skip(1).Select(x => $"{x.GetMethod().DeclaringType.FullName}.{x.GetMethod().Name}");
                v += "\n" + string.Join("\n", stackTrance);
            }

            File.AppendAllText(path, $"{DateTime.Now.ToString("yyyyMMdd HH:mm:ss")} [{pid}] [{level}] " + v + "\n");
        }

        internal static void exceptionLogCallback (string condition, string stackTrace, LogType type)
        {
            if(type == LogType.Exception)
            {
                ERRO(condition + "\n" + stackTrace);
            }
        }

        private static int pid = Tools.GRandom.getNum(10000, 99999);

        private static Log instance;
        private static string path = Application.streamingAssetsPath + "/log.txt";
    }
}
