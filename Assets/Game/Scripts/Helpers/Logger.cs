using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger
{
    static bool logEnabled = true;

    public static void Log(string Message)
    {
        if (logEnabled) Debug.Log(Message);
    }
}
