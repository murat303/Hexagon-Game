using UnityEngine;

namespace Hexagon
{
    public class Logger
    {
        static bool logEnabled = true;

        public static void Log(string Message)
        {
            if (logEnabled) Debug.Log(Message);
        }
    }
}
